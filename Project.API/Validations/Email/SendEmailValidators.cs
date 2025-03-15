namespace Project.API.Validations.Email
{
    public class SendEmailValidators : AbstractValidator<SendEmail>
    {


        public SendEmailValidators()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {

            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");

            RuleFor(x => x.Message).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");



        }
    }
}
