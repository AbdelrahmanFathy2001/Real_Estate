using Project.BLL.Dtos.Office;
using System.Security.Claims;

namespace Project.API.Controllers.Account
{

    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly IGenericRepository<UserImage> _repoImage;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor,
            IEmailServices emailServices, IApplicationUserServices applicationUserServices , IGenericRepository<UserImage> repoImage)
        {
            _userManager = userManager;
            _applicationUserServices = applicationUserServices;
            _repoImage = repoImage;
        }

        [HttpPost(Router.AccountRouting.Register)]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid data.", errors = ModelState });

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                City = model.City,
                Country = model.Country,
                Street = model.Street,
            };


            var result = await _applicationUserServices.AddUser(user, model.Password);

            return result switch
            {
                "EmailIsExist" => BadRequest(new { mess = "Email is already in use!" }),
                "UserNameIsExist" => BadRequest(new { mess = "Username is already in use!" }),
                "UserPhoneIsExist" => BadRequest(new { mess = "Phone number is already in use!" }),
                "FailedToTryRegister" => BadRequest(new { mess = "Registration failed. Please try again!" }),
                "SendEmailSucccess" => Ok(new { mess = "Customer added successfully!" }),
                _ => BadRequest(new { mess = "An unknown error occurred.", details = result })
            };
        }

        [Authorize(Roles = "Admin,REOffice")]
        [HttpPost(Router.AccountRouting.AddAcount)]
        public async Task<IActionResult> AddUserAsync([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid data.", errors = ModelState });

            var currentUser = HttpContext.User;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userManager.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest(new { mess = "Email is already in use!" });

            if (await _userManager.Users.AnyAsync(u => u.UserName == model.UserName))
                return BadRequest(new { mess = "Username is already in use!" });

            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.Phone))
                return BadRequest(new { mess = "Phone number is already in use!" });

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                City = model.City,
                Country = model.Country,
                Street = model.Street,
                EmailConfirmed = true
            };

            if (currentUser.IsInRole("REOffice"))
            {
                var reAgentCount = await _userManager.Users.CountAsync(u => u.SupervisorId == currentUserId);
                if (reAgentCount >= 2)
                    return BadRequest(new { mess = "You have exceeded the required number of employees, which is only two employees.!" });

                user.SupervisorId = currentUserId;
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { mess = "Failed to register user.", errors = result.Errors });

            if (currentUser.IsInRole("REOffice"))
            {
                await _userManager.AddToRoleAsync(user, "REOStaff");
            }


            return Ok(new { mess = "User registered successfully." });
        }

        [Authorize]
        [HttpPost(Router.AccountRouting.AddUserImage)]
        public async Task<IActionResult> AddUserImage([FromForm] ImageDto imageDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { mess = "الرجاء تسجيل الدخول" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { mess = "هذاالعميل غير موجود", info = "حاول مره اخرا!" });

            if (imageDto.ImageUrl != null)
            {
                if (imageDto.ImageUrl.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "يجب ألا يتجاوز حجم الصورة 5 ميجابايت." });
                }

                var existingImage = await _repoImage.FirstOrDefaultAsync(img => img.ApplicationUserId == userId);

                var fileName = DocumentSettings.UploadFile(imageDto.ImageUrl);

                if (existingImage != null)
                {
                    DocumentSettings.DeleteFile(existingImage.PictureUrl);

                    existingImage.PictureUrl = fileName;
                    await _repoImage.UpdateAsync(existingImage);
                }
                else
                {
                    var image = new UserImage
                    {
                        PictureUrl = fileName,
                        ApplicationUserId = userId
                    };
                    await _repoImage.AddAsync(image);
                }
            }

            return Ok(new { message = "تم اضافة الصوره بنجاح" });
        }




        [Authorize]
        [HttpPost(Router.AccountRouting.changePassword)]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid data.", errors = ModelState });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { mess = "Unauthorized request. User ID not found." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { mess = $"User with ID = {userId} does not exist.", info = "Try again!" });

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
                return Ok(new { mess = "Password changed successfully." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { mess = "Failed to change the password.", errors });
        }

        [Authorize]
        [HttpPut(Router.AccountRouting.Edit)]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid data.", errors = ModelState });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { mess = "Unauthorized request. User ID not found." });

            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser == null)
                return NotFound(new { mess = "User does not exist." });

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null && existingUser.Id != oldUser.Id)
                return BadRequest(new { mess = "Username is already in use by another account." });

            oldUser.FirstName = model.FirstName;
            oldUser.LastName = model.LastName;
            oldUser.UserName = model.UserName;
            oldUser.City = model.City;
            oldUser.Country = model.Country;
            oldUser.PhoneNumber = model.Phone;
            oldUser.Street = model.Street;

            var result = await _userManager.UpdateAsync(oldUser);

            return result.Succeeded
                ? Ok(new { mess = "User updated successfully." })
                : BadRequest(new { mess = "Failed to update the user." });
        }

        [Authorize]
        [HttpGet(Router.AccountRouting.GetUser)]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { mess = "Unauthorized request. User ID not found." });

            var user = await _applicationUserServices.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { mess = "User does not exist.", info = "Try again!" });
            return Ok(user);

        }

        [Authorize]
        [HttpDelete(Router.AccountRouting.DeleteAcount)]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { mess = "Unauthorized request. User ID not found." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { mess = "User does not exist.", info = "Try again!" });

            if (user.Inactive)
                return BadRequest(new { mess = "This user is already inactive." });

            user.Inactive = true;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded
                ? Ok(new { mess = "Account deactivated successfully." })
                : BadRequest(new { mess = "Failed to deactivate the account." });
        }

        [Authorize(Roles = "Admin,REOffice")]
        [HttpDelete(Router.AccountRouting.DeleteUser)]
        public async Task<IActionResult> DeleteUserAsync(string? id)
        {
            if (id.IsNullOrEmpty())
                return BadRequest(new { mess = "Invalid user ID." });

            var currentUser = HttpContext.User;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { mess = $"User with ID = {id} does not exist.", info = "Try again!" });

            if (currentUser.IsInRole("REOffice"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != user.SupervisorId)
                    return BadRequest(new { mess = "You are not authorized to delete this user." });

                var deleteResult = await _userManager.DeleteAsync(user);
                return deleteResult.Succeeded
                    ? Ok(new { mess = "User deleted successfully." })
                    : BadRequest(new { mess = "Failed to delete the user." });
            }

            if (user.Inactive)
                return BadRequest(new { mess = "This user is already inactive." });

            user.Inactive = true;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded
                ? Ok(new { mess = "User is now inactive." })
                : BadRequest(new { mess = "Failed to update the user status." });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut(Router.AccountRouting.UserActive)]
        public async Task<IActionResult> UserActivationAsync(string? id)
        {
            if (id.IsNullOrEmpty())
                return BadRequest(new { mess = "Invalid user ID." });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { mess = $"User with ID = {id} does not exist.", info = "Try again!" });

            if (!user.Inactive)
                return BadRequest(new { mess = "This user is already active." });

            user.Inactive = false;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded
                ? Ok(new { mess = "User has been activated successfully." })
                : BadRequest(new { mess = "Failed to activate the user." });
        }

    }
}
