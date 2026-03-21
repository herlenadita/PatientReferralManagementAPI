using FluentValidation;
using PatientReferralManagementAPI.DTO.Patient;
namespace PatientReferralManagementAPI.Validators
{
    public class PatientValidator : AbstractValidator<CreateUpdatePatientDto>
    {
        /**
         * TODO
         * - FirstName and LastName should only contain letters and spaces, and be between 2 and 200 characters long.
         * - DateOfBirth should be a valid date not more than today's date.
         */
        public PatientValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();
        }

    }
}
