using PFAOnboardingApi.Data.Queries;
using PFAOnboardingApi.DTOs;

namespace PFAOnboardingApi.Data.Queries;

public static class UserDetailsDtoMapper
{
    public static ExistingUserDetailsDto ToExistingUserDetailsDto(UserDetailsLookupRow row) =>
        new(
            row.UserId,
            row.Name,
            row.Mobile,
            row.EmailId,
            PanNo: null,
            AadhaarNumber: null,
            UanNumber: null);
}
