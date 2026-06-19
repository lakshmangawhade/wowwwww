namespace PFAOnboardingApi.Entities;

/// <summary>
/// Existing cc.UserDetails — lookup by Mobile for onboarding pre-fill.
/// PAN, Aadhaar, and UAN are collected on the onboarding form only (not stored here).
/// </summary>
public class UserDetails
{
    public int UserId { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? EmailId { get; set; }
    public bool? Active { get; set; }
}
