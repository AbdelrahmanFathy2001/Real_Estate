using Project.DAL.Entities.Identity;

namespace Project.BLL.Specifications
{
    public class UserWithFiltersForCountSpecification : BaseSpecification<ApplicationUser>
    {
        public UserWithFiltersForCountSpecification(UserSpecParams userSpecParams)
            : base(p =>
            (string.IsNullOrEmpty(userSpecParams.Search) || p.Email.ToLower().Contains(userSpecParams.Search))

            )
        {

        }
    }

}
