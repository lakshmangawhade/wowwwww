using FluentValidation;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.DTOs;
using PFAOnboardingApi.Helpers;

namespace PFAOnboardingApi.Validators;

public class SubmitOnboardingRequestValidator : AbstractValidator<SubmitOnboardingRequest>
{
    public SubmitOnboardingRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Must(IndianIdentityValidator.IsValidMobile)
            .WithMessage("Enter a valid 10-digit Indian mobile number.");

        RuleFor(x => x.EmailId)
            .NotEmpty().WithMessage("Email is required.")
            .Must(IndianIdentityValidator.IsValidEmail)
            .WithMessage("Enter a valid email address.")
            .MaximumLength(256);

        RuleFor(x => x.PanNo)
            .NotEmpty().WithMessage("PAN is required.")
            .Must(IndianIdentityValidator.IsValidPan)
            .WithMessage("Enter a valid PAN (e.g. ABCDE1234F).");

        RuleFor(x => x.AadhaarNumber)
            .NotEmpty().WithMessage("Aadhaar number is required.")
            .Must(IndianIdentityValidator.IsValidAadhaar)
            .WithMessage("Enter a valid 12-digit Aadhaar number.");

        RuleFor(x => x.UanNumber)
            .Must(IndianIdentityValidator.IsValidUan)
            .WithMessage("Enter a valid 12-digit UAN number or leave blank.");

        RuleFor(x => x.TerritoryId)
            .GreaterThan(0).WithMessage("Territory is required.");

        RuleFor(x => x.DistributorIds)
            .NotNull().WithMessage("Select at least one distributor.")
            .Must(ids => ids is { Count: > 0 })
            .WithMessage("Select at least one distributor.")
            .Must(ids => ids!.Distinct(StringComparer.OrdinalIgnoreCase).Count() == ids!.Count)
            .WithMessage("Duplicate distributors are not allowed.");

        RuleForEach(x => x.DistributorIds)
            .NotEmpty().WithMessage("Invalid distributor selected.")
            .MaximumLength(OnboardingConstants.DistributorIdMaxLength)
            .WithMessage($"Distributor identifier cannot exceed {OnboardingConstants.DistributorIdMaxLength} characters.");

        RuleFor(x => x.UserDetailsId)
            .NotNull()
            .When(x => x.UseExistingUserDetails)
            .WithMessage("UserDetailsId is required when using existing user details.");

        RuleFor(x => x.UserDetailsId)
            .Null()
            .When(x => !x.UseExistingUserDetails)
            .WithMessage("UserDetailsId must not be sent when not using existing user details.");
    }
}
