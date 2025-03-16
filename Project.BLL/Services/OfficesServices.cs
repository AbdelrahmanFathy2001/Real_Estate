using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Project.BLL.Dtos.Account;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class OfficesServices: IOfficesServices
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OfficesServices(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext context , IMapper mapper , IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<IReadOnlyList<GetAllUsersDto>> GetUsersByRoleAsync(string roleName)
        {
            // التأكد من أن الدور موجود
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception($"الدور '{roleName}' غير موجود.");
            }

            // جلب المستخدمين المرتبطين بالدور
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            // جلب الصور من قاعدة البيانات
            var userIds = users.Select(u => u.Id).ToList();
            var userImages = await _context.UserImage
                .Where(img => userIds.Contains(img.ApplicationUserId))
                .ToDictionaryAsync(img => img.ApplicationUserId, img => img.PictureUrl);


            var Data = users.Select(user => new GetAllUsersDto
            {
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OfficeName = user.OfficeName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Country = user.Country,
                City = user.City,
                Street = user.Street,
                ImageUrl = userImages.ContainsKey(user.Id) ? $"{_configuration["ApiUrl"]}{userImages[user.Id]}" : null // إضافة رابط API للصورة
            }).ToList();

            return Data;
        }

    }
}
