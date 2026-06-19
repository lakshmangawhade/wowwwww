using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.Entities;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Data.Queries;

public static class UserDetailsQueryExtensions
{
    public static IQueryable<UserDetails> WhereUserIsActive(this IQueryable<UserDetails> query) =>
        query.Where(u => u.Active == null || u.Active == true);
}
