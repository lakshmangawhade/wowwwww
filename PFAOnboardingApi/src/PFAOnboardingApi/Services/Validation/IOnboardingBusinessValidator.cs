using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Services.Validation;

public interface IOnboardingBusinessValidator
{
    Task ValidateAsync(
        SubmitOnboardingRequest request,
        string normalizedMobile,
        CancellationToken cancellationToken = default);
}
