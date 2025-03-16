

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Authorization;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        public async Task<string> AddRoleAsync(string RoleName)
        {
            var roleExist = await _roleManager.FindByNameAsync(RoleName);
            if (roleExist != null) return "RoleIsExist";

            Role role = new Role()
            {
                Name = RoleName,
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return "Created";
            }
            else
            {
                return "Failure";
            }
        }

        public async Task<string> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return "notfound";

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return "Delete";
            }
            else
            {
                return "Failed";
            }

        }

        public async Task<IReadOnlyList<ManageUsers>> GetUsersByRoleAsync(string? roleName)
        {
            // جلب جميع المستخدمين النشطين
            var users = await _userManager.Users.Where(u => u.EmailConfirmed == true).ToListAsync();

            // جلب جميع الأدوار من قاعدة البيانات
            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRolesList = new List<ManageUsers>();

            foreach (var user in users)
            {
                // جلب الأدوار الخاصة بالمستخدم
                var userRoles = await _userManager.GetRolesAsync(user);

                // تصفية المستخدمين بناءً على الدور المطلوب
                if ((!string.IsNullOrEmpty(roleName) && userRoles.Contains(roleName)) ||  // إذا كان له الدور المطلوب
                    (string.IsNullOrEmpty(roleName) && userRoles.Count == 0))  // أو إذا لم يكن لديه أي دور
                {
                    var response = new ManageUsers
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Country = user.Country,
                        City = user.City,
                        Street = user.Street,
                        Roles = userRoles.Select(role =>
                        {
                            var roleData = allRoles.FirstOrDefault(r => r.Name == role);
                            return new Roles
                            {
                                RoleId = roleData?.Id, // إرجاع RoleId إذا وجد الدور
                                RoleName = role,
                                HasRole = true
                            };
                        }).ToList()
                    };

                    userRolesList.Add(response);
                }
            }

            return userRolesList;
        }





        public async Task<ManageUserRoles> GetManageUserRoleWithRoles(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();
            var listRoles = new List<Roles>();

            var response = new ManageUserRoles
            {
                UserId = user.Id,
                username = user.UserName,
            };

            foreach (var role in roles)
            {
                var userRole = new Roles
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name)
                };

                listRoles.Add(userRole);
            }
            response.Roles = listRoles;

            return response;
        }

        public async Task<List<Role>> GetRolesAsync()
        {

            var Roles = await _roleManager.Roles.ToListAsync();
            return Roles;
        }

        public async Task<string> UpdateRoleAsync(string RoleName, string id)
        {
            var OldRole = await _roleManager.FindByIdAsync(id.ToString());
            if (OldRole == null) { return "NotFound"; }
            var RoleExist = await _roleManager.FindByNameAsync(RoleName);
            if (RoleExist != null && RoleExist.Id != id) { return "RoleExist"; }
            OldRole.Name = RoleName;
            var result = await _roleManager.UpdateAsync(OldRole);
            if (!result.Succeeded)
            {
                return "failure";
            }
            return "Update";
        }



        public async Task<string> UpdateUserRoleWithRoles(UpdateUserRole userRole)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                //get olduser
                var user = await _userManager.FindByIdAsync(userRole.UserId.ToString());
                if (user == null) { return "notfound"; }
                var userrole = await _userManager.GetRolesAsync(user);
                if (userrole != null)
                {
                    var RemoveResult = await _userManager.RemoveFromRolesAsync(user, userrole);
                    if (!RemoveResult.Succeeded)
                        return "FailedRemoveOldRoles";
                }
                var SelectedRoles = userRole.Roles.Where(x => x.HasRole == true).Select(x => x.RoleName);
                // addnewRoles
                var AddResult = await _userManager.AddToRolesAsync(user, SelectedRoles);
                if (!AddResult.Succeeded)
                    return "FailedAddNewRoles";
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }

        }

    }

}
