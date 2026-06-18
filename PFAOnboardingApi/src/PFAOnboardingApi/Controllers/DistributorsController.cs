using Microsoft.AspNetCore.Mvc;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Services;

namespace PFAOnboardingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DistributorsController : ControllerBase
{
    private readonly IDistributorService _distributorService;

    public DistributorsController(IDistributorService distributorService) =>
        _distributorService = distributorService;

    /// <summary>
    /// Returns distributor shop names for a territory (CustomerTypeID = 3).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DistributorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<DistributorDto>>> GetByTerritory(
        [FromQuery] int territoryId,
        CancellationToken cancellationToken)
    {
        if (territoryId <= 0)
            return BadRequest(new ApiErrorResponse("territoryId is required and must be greater than zero."));

        var distributors = await _distributorService.GetDistributorsByTerritoryAsync(
            territoryId,
            cancellationToken);

        return Ok(distributors);
    }
}
