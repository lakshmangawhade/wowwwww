namespace PFAOnboardingApi.DTOs;

public record TerritoryDto(int TerritoryId, string TerritoryName);

public record DistributorDto(int DealerId, string RetailerShopName);

public record UserLookupResponse(
    bool Found,
    string? Message,
    ExistingUserDetailsDto? UserDetails);

public record ExistingUserDetailsDto(
    int UserId,
    string? Name,
    string Mobile,
    string? EmailId,
    string? PanNo,
    string? AadhaarNumber,
    string? UanNumber);

public record SubmitOnboardingRequest(
    string Name,
    string Mobile,
    string EmailId,
    string PanNo,
    string AadhaarNumber,
    string? UanNumber,
    int TerritoryId,
    IReadOnlyList<int> DealerIds,
    bool UseExistingUserDetails,
    int? UserDetailsId);

public record OnboardingSubmissionResponse(
    long RequestId,
    string Status,
    string Message,
    int SelectedDistributorCount);

public record ApiErrorResponse(string Message, IReadOnlyDictionary<string, string[]>? Errors = null);
