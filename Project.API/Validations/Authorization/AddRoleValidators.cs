
using Project.BLL.Dtos.Authorization;

namespace Project.API.Validations.Authorization
{
    public class AddRoleValidators : AbstractValidator<AddRoleDto>
    {

        public AddRoleValidators()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {

            RuleFor(x => x.RoleName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null")
                .MaximumLength(10).WithMessage("{PropertyName} Is Max 10");



        }

        public void ApplyCustomValidationRules()
        {
            //RuleFor(x => x.Name).MustAsync(async (Key, CancellationToken) => !await _studentServices.IsNameExist(Key)).WithMessage("Already Name Is Exist");
        }




    }
}
