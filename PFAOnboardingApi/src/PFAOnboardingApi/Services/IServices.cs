using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Services;

public interface ITerritoryService
{
    Task<IReadOnlyList<TerritoryDto>> GetActiveTerritoriesAsync(CancellationToken cancellationToken = default);
}

public interface IDistributorService
{
    Task<IReadOnlyList<DistributorDto>> GetDistributorsByTerritoryAsync(
        int territoryId,
        CancellationToken cancellationToken = default);
}

public interface IUserLookupService
{
    Task<UserLookupResponse> LookupByMobileAsync(
        string mobile,
        CancellationToken cancellationToken = default);
}

public interface IOnboardingService
{
    Task<OnboardingSubmissionResponse> SubmitAsync(
        SubmitOnboardingRequest request,
        CancellationToken cancellationToken = default);
}
