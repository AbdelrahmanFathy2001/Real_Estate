
using Project.BLL.Dtos.Account;

namespace Project.API.Validations.Account
{
    public class UpdateUserValidators : AbstractValidator<UpdateUserDto>
    {


        public UpdateUserValidators()
        {
            ApplyValidationRules();

        }

        public void ApplyValidationRules()
        {
            // FirstName
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} must contain only letters and spaces.");

            // LastName
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} must contain only letters and spaces.");

            // UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(20).WithMessage("{PropertyName} must not exceed 20 characters.")
                .Matches(@"^[a-zA-Z]{3}[a-zA-Z0-9_]{0,17}$").WithMessage("{PropertyName} must start with 3 letters followed by alphanumeric or underscores.");

            // Phone
            RuleFor(x => x.Phone)
                .Matches(@"^(?:010|011|012|015)[0-9]{8}$|^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$")
                .WithMessage("{PropertyName} must be a valid Egyptian (010, 011, 012, 015) or Saudi (05) phone number and contain 8 digits after the prefix.");

            // Street
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s,]*$").WithMessage("{PropertyName} contains invalid characters.");

            // City
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} must contain only letters and spaces.");

            // Country
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} must contain only letters and spaces.");

        }
    }
}