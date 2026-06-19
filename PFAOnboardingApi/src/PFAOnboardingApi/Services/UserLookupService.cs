using PFAOnboardingApi.Data.Queries;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Services;

public class UserLookupService : IUserLookupService
{
    private readonly IUserDetailsLookupQuery _userDetailsLookup;
    private readonly ILogger<UserLookupService> _logger;

    public UserLookupService(
        IUserDetailsLookupQuery userDetailsLookup,
        ILogger<UserLookupService> logger)
    {
        _userDetailsLookup = userDetailsLookup;
        _logger = logger;
    }

    public async Task<UserLookupResponse> LookupByMobileAsync(
        string mobile,
        CancellationToken cancellationToken = default)
    {
        if (!IndianIdentityValidator.IsValidMobile(mobile))
        {
            return new UserLookupResponse(
                Found: false,
                Message: "Enter a valid 10-digit mobile number.",
                UserDetails: null);
        }

        var normalizedMobile = IndianIdentityValidator.NormalizeMobile(mobile);

        try
        {
            var row = await _userDetailsLookup.FindByMobileAsync(normalizedMobile, cancellationToken);

            if (row is null)
            {
                return new UserLookupResponse(
                    Found: false,
                    Message: "No existing user found for this mobile number. Please fill the form manually.",
                    UserDetails: null);
            }

            _logger.LogInformation(
                "UserDetails match found for mobile ending {MobileSuffix}, UserId {UserId}.",
                normalizedMobile[^4..],
                row.UserId);

            return new UserLookupResponse(
                Found: true,
                Message: "An existing profile was found. Name and email can be pre-filled; please enter PAN, Aadhaar, and UAN manually.",
                UserDetails: UserDetailsDtoMapper.ToExistingUserDetailsDto(row));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserDetails lookup failed for mobile ending {MobileSuffix}.", normalizedMobile[^4..]);
            throw;
        }
    }
}
