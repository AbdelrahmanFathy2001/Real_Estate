using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.BLL.Dtos.Account;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailServices _emailServices;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IUrlHelper Url;
        private readonly IConfiguration _configuration;

        public ApplicationUserServices(UserManager<ApplicationUser> userManager
            , IEmailServices emailServices
            , IHttpContextAccessor contextAccessor,
            ApplicationDbContext context,
             IUrlHelper Url, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailServices = emailServices;
            _contextAccessor = contextAccessor;
            _context = context;
            this.Url = Url;
            _configuration = configuration;
        }



        public async Task<string> AddUser(ApplicationUser user, string password)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                //the best be  in  flunent validation
                var SameEmail = await _userManager.FindByEmailAsync(user.Email);
                if (SameEmail != null)
                {
                    return "EmailIsExist";
                }
                var SameUserName = await _userManager.FindByNameAsync(user.UserName);
                if (SameUserName != null)
                {

                    return "UserNameIsExist";
                }
                var SameUserPhone = await _context.Users.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();
                if (SameUserPhone != null)
                {

                    return "UserPhoneIsExist";
                }


                // send confirm email

                IdentityResult Result = await _userManager.CreateAsync(user, password);
                if (!Result.Succeeded)
                    return string.Join(",", Result.Errors.Select(x => x.Description).ToList());
                //await _userManager.AddToRoleAsync(user, "user");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var requestaccessor = _contextAccessor.HttpContext.Request;
                var returnUrl = $"{requestaccessor.Scheme}://{requestaccessor.Host}" + Url.Action("ConfirmEmail", "Authentication", new
                {
                    UserId = user.Id,
                    Code = code,
                    ischeck = false
                });
                //$"/Api/Authentication/ConfirmEmail?UserId={user.Id}&Code={code}";
                var message = $"Welcome <b>{user.UserName}<b> To Confirm Email: <a href='{returnUrl}'>Click Here </a>";
                await _emailServices.SendEmailAsync(user.Email, message, "Confirm Email");
                await trans.CommitAsync();
                return "SendEmailSucccess";


            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "FailedToTryRegister";
            }
        }

        public async Task<GetAllUsersDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("هذا المستخدم غير موجود");
            }

            var userImage = await _context.UserImage
                .Where(img => img.ApplicationUserId == user.Id)
                .Select(img => img.PictureUrl)
                .FirstOrDefaultAsync();

            var userDto = new GetAllUsersDto
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
                ImageUrl = userImage != null ? $"{_configuration["ApiUrl"]}{userImage}" : null
            };

            return userDto;
        }

        public async Task<IReadOnlyList<GetAllUsersDto>> GetUsersWithoutRolesAsync()
        {
            // جلب جميع المستخدمين
            var allUsers = await _userManager.Users.ToListAsync();

            // تصفية المستخدمين الذين لا يملكون أي دور
            var usersWithoutRoles = new List<ApplicationUser>();
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || roles.Count == 0)
                {
                    usersWithoutRoles.Add(user);
                }
            }

            // جلب صور المستخدمين من قاعدة البيانات
            var userIds = usersWithoutRoles.Select(u => u.Id).ToList();
            var userImages = await _context.UserImage
                .Where(img => userIds.Contains(img.ApplicationUserId))
                .ToDictionaryAsync(img => img.ApplicationUserId, img => img.PictureUrl);

            // تحويل المستخدمين إلى `GetAllUsersDto`
            var Data = usersWithoutRoles.Select(user => new GetAllUsersDto
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
            }).ToList();

            return Data;
        }



    }

}