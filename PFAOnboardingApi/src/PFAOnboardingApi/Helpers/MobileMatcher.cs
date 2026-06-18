using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Helpers;

public static class MobileMatcher
{
    public static IQueryable<UserDetails> WhereMobileMatches(
        IQueryable<UserDetails> source,
        string normalizedMobile)
    {
        return source.Where(u =>
            u.Mobile == normalizedMobile ||
            u.Mobile == "91" + normalizedMobile ||
            u.Mobile == "+91" + normalizedMobile);
    }
}
