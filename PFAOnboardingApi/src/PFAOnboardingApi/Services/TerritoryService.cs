using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Services;

public class TerritoryService : ITerritoryService
{
    private readonly ApplicationDbContext _db;

    public TerritoryService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<TerritoryDto>> GetActiveTerritoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _db.TerritoryMaster
            .AsNoTracking()
            .Where(t => t.IsActive)
            .OrderBy(t => t.TerritoryName)
            .Select(t => new TerritoryDto(t.TerritoryId, t.TerritoryName))
            .ToListAsync(cancellationToken);
    }
}
