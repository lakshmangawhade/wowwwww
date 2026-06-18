using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Services;

namespace PFAOnboardingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OnboardingController : ControllerBase
{
    private readonly IOnboardingService _onboardingService;
    private readonly IValidator<SubmitOnboardingRequest> _validator;

    public OnboardingController(
        IOnboardingService onboardingService,
        IValidator<SubmitOnboardingRequest> validator)
    {
        _onboardingService = onboardingService;
        _validator = validator;
    }

    /// <summary>Submits the onboarding form. Distributors are stored in PFAOnboardingRequestDistributors.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OnboardingSubmissionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OnboardingSubmissionResponse>> Submit(
        [FromBody] SubmitOnboardingRequest request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(new ApiErrorResponse(
                "Validation failed.",
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));
        }

        var result = await _onboardingService.SubmitAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Submit), new { id = result.RequestId }, result);
    }
}
