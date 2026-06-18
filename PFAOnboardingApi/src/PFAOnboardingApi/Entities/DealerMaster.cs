namespace PFAOnboardingApi.Entities;

/// <summary>
/// Existing master table — distributors (CustomerTypeID = 3) per territory.
/// </summary>
public class DealerMaster
{
    public int DealerId { get; set; }
    public int TerritoryId { get; set; }
    public int CustomerTypeId { get; set; }
    public string RetailerShopName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public TerritoryMaster Territory { get; set; } = null!;
    public ICollection<PFAOnboardingRequestDistributor> OnboardingSelections { get; set; } = new List<PFAOnboardingRequestDistributor>();
}
