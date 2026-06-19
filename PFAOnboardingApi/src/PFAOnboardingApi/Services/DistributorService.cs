using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Data;
using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Services;

public class DistributorService : IDistributorService
{
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

        return await _db.DealerMaster
            .AsNoTracking()
            .Where(d =>
                d.TerritoryId == territoryId &&
                d.CustomerTypeId == OnboardingConstants.DistributorCustomerTypeId &&
                (d.IsActive == null || d.IsActive == true))
            .OrderBy(d => d.RetailerShopName)
            .Select(d => new DistributorDto(d.ContactId, d.RetailerShopName))
            .ToListAsync(cancellationToken);
    }
}
