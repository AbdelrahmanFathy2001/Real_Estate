using Project.BLL.Dtos.Authorization;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public interface IAuthorizationService
    {
        public Task<string> AddRoleAsync(string RoleName);
        public Task<string> UpdateRoleAsync(string RoleName, string id);
        public Task<List<Role>> GetRolesAsync();
        public Task<string> DeleteRoleAsync(string id);
        public Task<ManageUserRoles> GetManageUserRoleWithRoles(ApplicationUser user);
        public Task<string> UpdateUserRoleWithRoles(UpdateUserRole userRole);

        public Task<IReadOnlyList<ManageUsers>> GetUsersByRoleAsync(string? roleName);


    }
}
