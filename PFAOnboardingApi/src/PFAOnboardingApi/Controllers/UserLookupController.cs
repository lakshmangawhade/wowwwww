using Microsoft.AspNetCore.Mvc;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Services;

namespace PFAOnboardingApi.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserLookupController : ControllerBase
{
    private readonly IUserLookupService _userLookupService;

    public UserLookupController(IUserLookupService userLookupService) =>
        _userLookupService = userLookupService;

    /// <summary>
    /// Checks UserDetails by mobile. If found, frontend should prompt to reuse existing profile.
    /// </summary>
    [HttpGet("lookup")]
    [ProducesResponseType(typeof(UserLookupResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserLookupResponse>> LookupByMobile(
        [FromQuery] string mobile,
        CancellationToken cancellationToken)
    {
        var result = await _userLookupService.LookupByMobileAsync(mobile, cancellationToken);
        return Ok(result);
    }
}
