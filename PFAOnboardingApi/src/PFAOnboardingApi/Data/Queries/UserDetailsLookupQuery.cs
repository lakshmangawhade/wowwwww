using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Data.Queries;

public class UserDetailsLookupQuery : IUserDetailsLookupQuery
{
    private readonly ApplicationDbContext _db;

    public UserDetailsLookupQuery(ApplicationDbContext db) => _db = db;

    public async Task<UserDetailsLookupRow?> FindByMobileAsync(
        string normalizedMobile,
        CancellationToken cancellationToken = default)
    {
        return await MobileMatcher
            .WhereMobileMatches(_db.UserDetails.AsNoTracking(), normalizedMobile)
            .WhereUserIsActive()
            .Select(u => new UserDetailsLookupRow
            {
                UserId = u.UserId,
                Name = u.Name,
                Mobile = u.Mobile,
                EmailId = u.EmailId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDetailsLookupRow?> FindByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _db.UserDetails
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .WhereUserIsActive()
            .Select(u => new UserDetailsLookupRow
            {
                UserId = u.UserId,
                Name = u.Name,
                Mobile = u.Mobile,
                EmailId = u.EmailId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
