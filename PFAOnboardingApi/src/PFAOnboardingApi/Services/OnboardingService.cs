using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Entities;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Services;

public class OnboardingService : IOnboardingService
{
    private const int DistributorCustomerTypeId = 3;

    private readonly ApplicationDbContext _db;
    private readonly ILogger<OnboardingService> _logger;

    public OnboardingService(ApplicationDbContext db, ILogger<OnboardingService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<OnboardingSubmissionResponse> SubmitAsync(
        SubmitOnboardingRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedMobile = IndianIdentityValidator.NormalizeMobile(request.Mobile);
        var normalizedPan = IndianIdentityValidator.NormalizePan(request.PanNo);
        var normalizedAadhaar = IndianIdentityValidator.NormalizeAadhaar(request.AadhaarNumber);
        var normalizedUan = IndianIdentityValidator.NormalizeUan(request.UanNumber);

        await ValidateBusinessRulesAsync(
            request,
            normalizedMobile,
            cancellationToken);

        var distinctDealerIds = request.DealerIds.Distinct().ToList();

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var onboardingRequest = new PFAOnboardingRequest
            {
                Name = request.Name.Trim(),
                Mobile = normalizedMobile,
                EmailId = request.EmailId.Trim(),
                PanNo = normalizedPan,
                AadhaarNumber = normalizedAadhaar,
                UanNumber = normalizedUan,
                TerritoryId = request.TerritoryId,
                UsedExistingUserDetails = request.UseExistingUserDetails,
                UserDetailsId = request.UseExistingUserDetails ? request.UserDetailsId : null,
                CreatedAtUtc = DateTime.UtcNow,
                Status = OnboardingStatus.Pending
            };

            _db.PFAOnboardingRequests.Add(onboardingRequest);
            await _db.SaveChangesAsync(cancellationToken);

            var distributorRows = distinctDealerIds.Select(dealerId => new PFAOnboardingRequestDistributor
            {
                RequestId = onboardingRequest.RequestId,
                DealerId = dealerId,
                CreatedAtUtc = DateTime.UtcNow
            });

            _db.PFAOnboardingRequestDistributors.AddRange(distributorRows);
            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Onboarding request {RequestId} created for territory {TerritoryId} with {DealerCount} distributors.",
                onboardingRequest.RequestId,
                onboardingRequest.TerritoryId,
                distinctDealerIds.Count);

            return new OnboardingSubmissionResponse(
                onboardingRequest.RequestId,
                onboardingRequest.Status,
                "Onboarding request submitted successfully.",
                distinctDealerIds.Count);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task ValidateBusinessRulesAsync(
        SubmitOnboardingRequest request,
        string normalizedMobile,
        CancellationToken cancellationToken)
    {
        var territoryValid = await _db.TerritoryMaster
            .AsNoTracking()
            .AnyAsync(t => t.TerritoryId == request.TerritoryId && t.IsActive, cancellationToken);

        if (!territoryValid)
            throw new InvalidOperationException("Selected territory is invalid or inactive.");

        var distinctDealerIds = request.DealerIds.Distinct().ToList();

        var validDealers = await _db.DealerMaster
            .AsNoTracking()
            .Where(d =>
                d.TerritoryId == request.TerritoryId &&
                d.CustomerTypeId == DistributorCustomerTypeId &&
                distinctDealerIds.Contains(d.DealerId) &&
                (d.IsActive == null || d.IsActive == true))
            .Select(d => d.DealerId)
            .ToListAsync(cancellationToken);

        if (validDealers.Count != distinctDealerIds.Count)
            throw new InvalidOperationException(
                "One or more selected distributors are invalid for the chosen territory.");

        if (request.UseExistingUserDetails)
        {
            var linkedUser = await _db.UserDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.UserId == request.UserDetailsId &&
                         (u.IsActive == null || u.IsActive == true),
                    cancellationToken);

            if (linkedUser is null)
                throw new InvalidOperationException("Linked user details could not be found.");

            var linkedMobile = IndianIdentityValidator.NormalizeMobile(linkedUser.Mobile);
            if (!string.Equals(linkedMobile, normalizedMobile, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "Mobile number does not match the selected existing user profile.");
        }
        else
        {
            var existingUserQuery = MobileMatcher.WhereMobileMatches(
                _db.UserDetails.AsNoTracking(),
                normalizedMobile);

            var existingUser = await existingUserQuery
                .AnyAsync(u => u.IsActive == null || u.IsActive == true, cancellationToken);

            // Informational only — frontend should prompt; we still allow manual entry
            if (existingUser)
                _logger.LogInformation(
                    "Onboarding submitted without reusing existing UserDetails for mobile ending {MobileSuffix}.",
                    normalizedMobile[^4..]);
        }

        var duplicatePending = await _db.PFAOnboardingRequests
            .AsNoTracking()
            .AnyAsync(
                r => r.Mobile == normalizedMobile && r.Status == OnboardingStatus.Pending,
                cancellationToken);

        if (duplicatePending)
            throw new InvalidOperationException(
                "A pending onboarding request already exists for this mobile number.");
    }
}
