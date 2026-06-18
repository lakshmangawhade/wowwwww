namespace PFAOnboardingApi.Entities;

/// <summary>
/// Junction table for multiple distributor selections per onboarding request.
/// Suggested name: PFAOnboardingRequestDistributors (clear parent-child relationship).
/// </summary>
public class PFAOnboardingRequestDistributor
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public int DealerId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public PFAOnboardingRequest Request { get; set; } = null!;
    public DealerMaster Dealer { get; set; } = null!;
}
