
using Project.BLL.Dtos.Authentication;

namespace Project.API.Validations.Authenication
{
    public class LoginValidators : AbstractValidator<LoginDto>
    {
        public LoginValidators()
        {
            ApplyValtionRules();
        }


        public void ApplyValtionRules()
        {
            RuleFor(x => x.EmailOrUserName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null").WithName("Email Or UserName");

            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");
        }
    }
}
