namespace PFAOnboardingApi.Entities;

/// <summary>
/// Existing user table — used to pre-fill onboarding when mobile already exists.
/// Adjust column names to match your actual UserDetails schema if they differ.
/// </summary>
public class UserDetails
{
    public int UserId { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? EmailId { get; set; }
    public string? PanNo { get; set; }
    public string? AadhaarNumber { get; set; }
    public string? UanNumber { get; set; }
    public bool? IsActive { get; set; }
}
