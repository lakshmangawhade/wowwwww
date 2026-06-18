namespace PFAOnboardingApi.Entities;

/// <summary>
/// Main onboarding request — one row per form submission.
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

    /// <summary>True when the applicant chose to reuse data from UserDetails.</summary>
    public bool UsedExistingUserDetails { get; set; }

    /// <summary>FK to UserDetails when UsedExistingUserDetails is true.</summary>
    public int? UserDetailsId { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public string Status { get; set; } = OnboardingStatus.Pending;

    public TerritoryMaster Territory { get; set; } = null!;
    public UserDetails? LinkedUser { get; set; }
    public ICollection<PFAOnboardingRequestDistributor> SelectedDistributors { get; set; } = new List<PFAOnboardingRequestDistributor>();
}

public static class OnboardingStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
}
