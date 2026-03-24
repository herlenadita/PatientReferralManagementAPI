using FluentValidation;
using PatientReferralManagementAPI.DTO.Patient;
using System.Globalization;
namespace PatientReferralManagementAPI.Validators
{
    public class CreateUpdatePatientValidator : AbstractValidator<CreateUpdatePatientDto>
    {
        /**
         * TODO
         * - FirstName and LastName should only contain letters and spaces, and be between 2 and 200 characters long.
         * - DateOfBirth should be a valid date not more than today's date.
         */
        public CreateUpdatePatientValidator()
        {
            // FirstName
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 200).WithMessage("First name must be between 2 and 200 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces");

            // LastName
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 200).WithMessage("Last name must be between 2 and 200 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces");

            // DateOfBirth
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")

                .Must(date => DateTime.TryParseExact(
                    date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _))
                .WithMessage("Date of birth must be in format yyyy-MM-dd")

                .Must(date =>
                {
                    if (!DateTime.TryParseExact(
                        date,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var parsedDate))
                        return true;

                    return parsedDate.Date <= DateTime.Today;
                })
                .WithMessage("Date of birth cannot be in the future");
        }

    }
}
