namespace Project.API.Validations.Authenication
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmail>
    {
        public ConfirmEmailValidator()
        {
            ApplyValtionRules();
        }
        public void ApplyValtionRules()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");

            RuleFor(x => x.Code).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");
        }
    }
}
