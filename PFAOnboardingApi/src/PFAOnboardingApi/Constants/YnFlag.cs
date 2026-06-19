namespace PFAOnboardingApi.Constants;

/// <summary>
/// Legacy Y/N flag values used in master tables (e.g. cc.UserDetails.Active).
/// </summary>
public static class YnFlag
{
    public const string Yes = "Y";
    public const string No = "N";

    public static bool IsActive(string? value) =>
        value is null || string.Equals(value.Trim(), Yes, StringComparison.OrdinalIgnoreCase);
}
