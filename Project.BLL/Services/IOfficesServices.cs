using Project.BLL.Dtos.Account;
using Project.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public interface IOfficesServices
    {
        public Task<IReadOnlyList<GetAllUsersDto>> GetUsersByRoleAsync(string roleName);
    }
}
