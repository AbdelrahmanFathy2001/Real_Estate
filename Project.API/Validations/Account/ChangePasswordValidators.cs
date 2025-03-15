namespace Project.API.Validations.Account
{
    public class ChangePasswordValidators : AbstractValidator<ChangePasswordDto>
    {

        public ChangePasswordValidators()
        {
            ApplyValidationRules();
        }


        public void ApplyValidationRules()
        {

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("{PropertyName} is not empty")
                .NotNull().WithMessage("{PropertyName} is not null")
                .Matches(@"^[a-zA-Z0-9@#$%^&+=]{8,}$")
                .WithMessage("{PropertyName} contains invalid characters or does not meet complexity requirements.")
                .WithName("Current Password");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is not empty")
                .NotNull().WithMessage("{PropertyName} is not null")
                .Matches(@"^[a-zA-Z0-9@#$%^&+=]{8,}$")
                .WithMessage("{PropertyName} must be at least 8 characters long and can only contain letters, numbers, and special characters (@, #, $, %, ^, &, +, =).")
                .WithName("New Password");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is not empty")
                .NotNull().WithMessage("{PropertyName} is not null")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.")
                .Matches(@"^[a-zA-Z0-9@#$%^&+=]{8,}$")
                .WithMessage("{PropertyName} contains invalid characters or does not meet complexity requirements.")
                .WithName("Confirm Password");
        }


    }
}
