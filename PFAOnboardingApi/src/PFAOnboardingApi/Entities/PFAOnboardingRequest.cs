namespace PFAOnboardingApi.Entities;

/// <summary>
/// Main onboarding request stored in cc.PFAOnboardingRequests.
/// </summary>
public class PFAOnboardingRequest
{
    public long RequestId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string PanNo { get; set; } = string.Empty;
    public string AadhaarNumber { get; set; } = string.Empty;
    public string? UanNumber { get; set; }
    public int TerritoryId { get; set; }
    public bool UsedExistingUserDetails { get; set; }
    public int? UserDetailsId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public TerritoryMaster Territory { get; set; } = null!;
    public UserDetails? LinkedUser { get; set; }
    public ICollection<PFAOnboardingRequestDistributor> SelectedDistributors { get; set; } = new List<PFAOnboardingRequestDistributor>();
}
