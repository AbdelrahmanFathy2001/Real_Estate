using Project.BLL.Dtos.Account;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public interface IApplicationUserServices
    {
        public Task<string> AddUser(ApplicationUser user, string Password);
        public Task<GetAllUsersDto> GetUserByIdAsync(string id);

        public Task<IReadOnlyList<GetAllUsersDto>> GetUsersWithoutRolesAsync();

    }
}
