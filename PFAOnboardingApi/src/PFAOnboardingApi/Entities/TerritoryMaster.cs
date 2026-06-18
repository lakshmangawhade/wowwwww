namespace PFAOnboardingApi.Entities;

/// <summary>
/// Existing master table — active territories for the onboarding dropdown.
/// </summary>
public class TerritoryMaster
{
    public int TerritoryId { get; set; }
    public string TerritoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<DealerMaster> Dealers { get; set; } = new List<DealerMaster>();
    public ICollection<PFAOnboardingRequest> OnboardingRequests { get; set; } = new List<PFAOnboardingRequest>();
}
