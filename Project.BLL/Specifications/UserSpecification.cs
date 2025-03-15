using Project.DAL.Entities.Identity;

namespace Project.BLL.Specifications
{
    public class UserSpecification : BaseSpecification<ApplicationUser>
    {
        public UserSpecification(UserSpecParams userSpec)
            : base(p =>
                (string.IsNullOrEmpty(userSpec.Search) || p.Email.ToLower().Contains(userSpec.Search))            )
        {
            ApplyPagination(userSpec.PageSize * (userSpec.PageIndex - 1), userSpec.PageSize);
        }
    }



}
