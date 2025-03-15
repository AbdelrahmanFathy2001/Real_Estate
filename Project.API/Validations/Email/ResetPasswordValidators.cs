namespace Project.API.Validations.Email
{
    public class ResetPasswordValidators : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidators()
        {
            ApplyValidationRules();
        }
        public void ApplyValidationRules()
        {

            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null")
          .Matches(@"^[a-zA-Z]{3,}[a-zA-Z._0-9]{0,20}[a-zA-Z0-9]{1,}(@gmail.com|@yahoo.com)$")
          .WithMessage("Email must start with at least three alphabetic characters,allowed only '._' and numbers. Example :user123@(gmail.com|yahoo.com)");
        }
    }
}