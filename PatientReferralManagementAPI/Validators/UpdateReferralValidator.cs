using FluentValidation;
using PatientReferralManagementAPI.DTO.Referral;
namespace PatientReferralManagementAPI.Validators
{
    public class UpdateReferralValidator : AbstractValidator<UpdateReferralDto>
    {
        public UpdateReferralValidator()
        {
            RuleFor(x => x.ReferralSource)
                .NotEmpty();

            RuleFor(x => x.ReferralType)
                .NotEmpty();

            RuleFor(x => x.ReferralNote)
                .NotEmpty();
        }
    }
}
