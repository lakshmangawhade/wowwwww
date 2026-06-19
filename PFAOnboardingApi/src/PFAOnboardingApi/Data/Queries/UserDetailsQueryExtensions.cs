using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Queries;

public static class UserDetailsQueryExtensions
{
    /// <summary>
    /// Active users have Active = 'Y' (or null). Inactive rows use Active = 'N'.
    /// </summary>
    public static IQueryable<UserDetails> WhereUserIsActive(this IQueryable<UserDetails> query) =>
        query.Where(u =>
            u.Active == null ||
            u.Active == YnFlag.Yes ||
            u.Active == "y");
}
