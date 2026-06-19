namespace PFAOnboardingApi.Data.Queries;

public interface IUserDetailsLookupQuery
{
    Task<UserDetailsLookupRow?> FindByMobileAsync(
        string normalizedMobile,
        CancellationToken cancellationToken = default);

    Task<UserDetailsLookupRow?> FindByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);
}
