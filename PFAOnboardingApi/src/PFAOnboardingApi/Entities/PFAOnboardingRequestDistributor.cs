namespace PFAOnboardingApi.Entities;

/// <summary>
/// Selected distributor for an onboarding request (cc.PFAOnboardingRequestDistributors).
/// </summary>
public class PFAOnboardingRequestDistributor
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public string DistributorId { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

    public PFAOnboardingRequest Request { get; set; } = null!;
    public DealerMaster Distributor { get; set; } = null!;
}
