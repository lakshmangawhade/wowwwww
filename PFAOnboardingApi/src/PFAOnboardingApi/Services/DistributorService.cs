using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Services;

public class DistributorService : IDistributorService
{
    private const int DistributorCustomerTypeId = 3;

    private readonly ApplicationDbContext _db;

    public DistributorService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<DistributorDto>> GetDistributorsByTerritoryAsync(
        int territoryId,
        CancellationToken cancellationToken = default)
    {
        if (territoryId <= 0)
            return Array.Empty<DistributorDto>();

        var territoryExists = await _db.TerritoryMaster
            .AsNoTracking()
            .AnyAsync(t => t.TerritoryId == territoryId && t.IsActive, cancellationToken);

        if (!territoryExists)
            return Array.Empty<DistributorDto>();

        var query = _db.DealerMaster
            .AsNoTracking()
            .Where(d => d.TerritoryId == territoryId && d.CustomerTypeId == DistributorCustomerTypeId);

        // Filter active dealers only when the column exists and is populated
        query = query.Where(d => d.IsActive == null || d.IsActive == true);

        return await query
            .OrderBy(d => d.RetailerShopName)
            .Select(d => new DistributorDto(d.DealerId, d.RetailerShopName))
            .ToListAsync(cancellationToken);
    }
}
