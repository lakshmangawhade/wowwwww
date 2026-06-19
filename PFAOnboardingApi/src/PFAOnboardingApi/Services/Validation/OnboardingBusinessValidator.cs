using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.Data.Queries;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Services.Validation;

public class OnboardingBusinessValidator : IOnboardingBusinessValidator
{
    private readonly ApplicationDbContext _db;
    private readonly IUserDetailsLookupQuery _userDetailsLookup;
    private readonly ILogger<OnboardingBusinessValidator> _logger;

    public OnboardingBusinessValidator(
        ApplicationDbContext db,
        IUserDetailsLookupQuery userDetailsLookup,
        ILogger<OnboardingBusinessValidator> logger)
    {
        _db = db;
        _userDetailsLookup = userDetailsLookup;
        _logger = logger;
    }

    public async Task ValidateAsync(
        SubmitOnboardingRequest request,
        string normalizedMobile,
        CancellationToken cancellationToken = default)
    {
        await ValidateTerritoryAsync(request.TerritoryId, cancellationToken);
        await ValidateDistributorsAsync(request, cancellationToken);
        await ValidateUserDetailsLinkAsync(request, normalizedMobile, cancellationToken);
        await ValidateDuplicateMobileAsync(normalizedMobile, cancellationToken);
    }

    private async Task ValidateTerritoryAsync(int territoryId, CancellationToken cancellationToken)
    {
        var isValid = await _db.TerritoryMaster
            .AsNoTracking()
            .AnyAsync(t => t.TerritoryId == territoryId && t.IsActive, cancellationToken);

        if (!isValid)
            throw new InvalidOperationException("Selected territory is invalid or inactive.");
    }

    private async Task ValidateDistributorsAsync(
        SubmitOnboardingRequest request,
        CancellationToken cancellationToken)
    {
        var distributorIds = request.DistributorIds
            .Select(id => id.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var validCount = await _db.DealerMaster
            .AsNoTracking()
            .CountAsync(
                d => d.TerritoryId == request.TerritoryId &&
                     d.CustomerTypeId == OnboardingConstants.DistributorCustomerTypeId &&
                     distributorIds.Contains(d.ContactId) &&
                     (d.IsActive == null || d.IsActive == true),
                cancellationToken);

        if (validCount != distributorIds.Count)
            throw new InvalidOperationException(
                "One or more selected distributors are invalid for the chosen territory.");
    }

    private async Task ValidateUserDetailsLinkAsync(
        SubmitOnboardingRequest request,
        string normalizedMobile,
        CancellationToken cancellationToken)
    {
        if (request.UseExistingUserDetails)
        {
            if (!request.UserDetailsId.HasValue)
                throw new InvalidOperationException("UserDetailsId is required when using existing user details.");

            var linkedUser = await _userDetailsLookup.FindByUserIdAsync(
                request.UserDetailsId.Value,
                cancellationToken);

            if (linkedUser is null)
                throw new InvalidOperationException("Linked user details could not be found.");

            var linkedMobile = IndianIdentityValidator.NormalizeMobile(linkedUser.Mobile);
            if (!string.Equals(linkedMobile, normalizedMobile, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "Mobile number does not match the selected existing user profile.");

            return;
        }

        var existingUser = await _userDetailsLookup.FindByMobileAsync(normalizedMobile, cancellationToken);

        if (existingUser is not null)
        {
            _logger.LogInformation(
                "Onboarding submitted without reusing existing UserDetails for mobile ending {MobileSuffix}.",
                normalizedMobile[^4..]);
        }
    }

    private async Task ValidateDuplicateMobileAsync(
        string normalizedMobile,
        CancellationToken cancellationToken)
    {
        var alreadyExists = await _db.PFAOnboardingRequests
            .AsNoTracking()
            .AnyAsync(r => r.Mobile == normalizedMobile, cancellationToken);

        if (alreadyExists)
            throw new InvalidOperationException(
                "An onboarding request already exists for this mobile number.");
    }
}
