namespace PFAOnboardingApi.Data.Queries;

/// <summary>
/// Lookup result from cc.UserDetails. PAN / Aadhaar / UAN are always null (not in UserDetails).
/// </summary>
public class UserDetailsLookupRow
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string? EmailId { get; set; }
}
