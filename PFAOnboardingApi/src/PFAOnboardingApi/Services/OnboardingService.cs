using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Entities;
using PFAOnboardingApi.Helpers;
using PFAOnboardingApi.Services.Validation;

namespace PFAOnboardingApi.Services;

public class OnboardingService : IOnboardingService
{
    private readonly ApplicationDbContext _db;
    private readonly IOnboardingBusinessValidator _businessValidator;
    private readonly ILogger<OnboardingService> _logger;

    public OnboardingService(
        ApplicationDbContext db,
        IOnboardingBusinessValidator businessValidator,
        ILogger<OnboardingService> logger)
    {
        _db = db;
        _businessValidator = businessValidator;
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

        await _businessValidator.ValidateAsync(request, normalizedMobile, cancellationToken);

        var distributorIds = request.DistributorIds
            .Select(id => id.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var onboardingRequest = BuildOnboardingRequest(
                request,
                normalizedMobile,
                normalizedPan,
                normalizedAadhaar,
                normalizedUan);

            _db.PFAOnboardingRequests.Add(onboardingRequest);
            await _db.SaveChangesAsync(cancellationToken);

            var distributorRows = distributorIds.Select(distributorId => new PFAOnboardingRequestDistributor
            {
                RequestId = onboardingRequest.RequestId,
                DistributorId = distributorId,
                CreatedAtUtc = DateTime.UtcNow
            });

            _db.PFAOnboardingRequestDistributors.AddRange(distributorRows);
            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Onboarding request {RequestId} created for territory {TerritoryId} with {DistributorCount} distributors.",
                onboardingRequest.RequestId,
                onboardingRequest.TerritoryId,
                distributorIds.Count);

            return new OnboardingSubmissionResponse(
                onboardingRequest.RequestId,
                "Onboarding request submitted successfully.",
                distributorIds.Count);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static PFAOnboardingRequest BuildOnboardingRequest(
        SubmitOnboardingRequest request,
        string normalizedMobile,
        string normalizedPan,
        string normalizedAadhaar,
        string? normalizedUan)
    {
        return new PFAOnboardingRequest
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
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
