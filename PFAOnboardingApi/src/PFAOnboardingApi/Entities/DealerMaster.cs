namespace PFAOnboardingApi.Entities;

/// <summary>
/// Distributor record from cc.DealerMaster (CustomerTypeID = 3).
/// </summary>
public class DealerMaster
{
    public string ContactId { get; set; } = string.Empty;
    public int TerritoryId { get; set; }
    public int CustomerTypeId { get; set; }
    public string RetailerShopName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public TerritoryMaster Territory { get; set; } = null!;
    public ICollection<PFAOnboardingRequestDistributor> OnboardingSelections { get; set; } = new List<PFAOnboardingRequestDistributor>();
}
