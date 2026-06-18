using System.Text.RegularExpressions;

namespace PFAOnboardingApi.Helpers;

public static partial class IndianIdentityValidator
{
  private static readonly Regex MobileRegex = MobilePattern();
    private static readonly Regex EmailRegex = EmailPattern();
    private static readonly Regex PanRegex = PanPattern();
    private static readonly Regex AadhaarRegex = AadhaarPattern();
    private static readonly Regex UanRegex = UanPattern();

    public static string NormalizeMobile(string mobile)
    {
        var digits = new string(mobile.Where(char.IsDigit).ToArray());
        return digits.Length > 10 ? digits[^10..] : digits;
    }

    public static bool IsValidMobile(string mobile)
    {
        var normalized = NormalizeMobile(mobile);
        return normalized.Length == 10 && MobileRegex.IsMatch(normalized);
    }

    public static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());

    public static bool IsValidPan(string pan) =>
        !string.IsNullOrWhiteSpace(pan) && PanRegex.IsMatch(pan.Trim().ToUpperInvariant());

    public static bool IsValidAadhaar(string aadhaar)
    {
        var digits = new string(aadhaar.Where(char.IsDigit).ToArray());
        return digits.Length == 12 && AadhaarRegex.IsMatch(digits);
    }

    public static bool IsValidUan(string? uan)
    {
        if (string.IsNullOrWhiteSpace(uan)) return true;
        var digits = new string(uan.Where(char.IsDigit).ToArray());
        return digits.Length == 12 && UanRegex.IsMatch(digits);
    }

    public static string NormalizePan(string pan) => pan.Trim().ToUpperInvariant();

    public static string NormalizeAadhaar(string aadhaar) =>
        new string(aadhaar.Where(char.IsDigit).ToArray());

    public static string? NormalizeUan(string? uan)
    {
        if (string.IsNullOrWhiteSpace(uan)) return null;
        return new string(uan.Where(char.IsDigit).ToArray());
    }

    public static string MaskPan(string pan) =>
        pan.Length >= 6 ? $"{pan[..2]}****{pan[^4..]}" : "****";

    public static string MaskAadhaar(string aadhaar) =>
        aadhaar.Length == 12 ? $"XXXX-XXXX-{aadhaar[^4..]}" : "XXXX-XXXX-XXXX";

    [GeneratedRegex(@"^[6-9]\d{9}$", RegexOptions.Compiled)]
    private static partial Regex MobilePattern();

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"^[A-Z]{5}[0-9]{4}[A-Z]$", RegexOptions.Compiled)]
    private static partial Regex PanPattern();

    [GeneratedRegex(@"^\d{12}$", RegexOptions.Compiled)]
    private static partial Regex AadhaarPattern();

    [GeneratedRegex(@"^\d{12}$", RegexOptions.Compiled)]
    private static partial Regex UanPattern();
}
