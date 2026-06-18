using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Services;

public class UserLookupService : IUserLookupService
{
    private readonly ApplicationDbContext _db;

    public UserLookupService(ApplicationDbContext db) => _db = db;

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

        var userQuery = MobileMatcher.WhereMobileMatches(
            _db.UserDetails.AsNoTracking(),
            u => u.Mobile,
            normalizedMobile);

        var user = await userQuery
            .Where(u => u.IsActive == null || u.IsActive == true)
            .Select(u => new ExistingUserDetailsDto(
                u.UserId,
                u.Name,
                u.Mobile,
                u.EmailId,
                u.PanNo,
                u.AadhaarNumber,
                u.UanNumber))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return new UserLookupResponse(
                Found: false,
                Message: "No existing user found for this mobile number. Please fill the form manually.",
                UserDetails: null);
        }

        return new UserLookupResponse(
            Found: true,
            Message: "An existing profile was found. You can use these details to pre-fill the form.",
            UserDetails: user);
    }
}
