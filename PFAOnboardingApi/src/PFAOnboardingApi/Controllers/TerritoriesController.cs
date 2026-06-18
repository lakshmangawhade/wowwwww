using Microsoft.AspNetCore.Mvc;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Services;

namespace PFAOnboardingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TerritoriesController : ControllerBase
{
    private readonly ITerritoryService _territoryService;

    public TerritoriesController(ITerritoryService territoryService) =>
        _territoryService = territoryService;

    /// <summary>Returns all territories where IsActive = 1 for the dropdown.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TerritoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TerritoryDto>>> GetActiveTerritories(
        CancellationToken cancellationToken)
    {
        var territories = await _territoryService.GetActiveTerritoriesAsync(cancellationToken);
        return Ok(territories);
    }
}
