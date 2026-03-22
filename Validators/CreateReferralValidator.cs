using FluentValidation;
using PatientReferralManagementAPI.DTO.Referral;

namespace PatientReferralManagementAPI.Validators
{
    public class CreateReferralValidator : AbstractValidator<CreateReferralDto>
    {
        public CreateReferralValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be valid");

            RuleFor(x => x.ReferralSource)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ReferralType)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ReferralNote)
                .NotEmpty();
        }
    }
}
