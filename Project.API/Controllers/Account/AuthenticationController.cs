


using Microsoft.AspNetCore.Identity;

namespace Project.API.Controllers.Account
{

    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthenticationServices _authServices;
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager,
            IAuthenticationServices authServices, 
            IAuthorizationService authorizationService,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
            _authorizationService = authorizationService;
            _roleManager = roleManager;
        }

        [HttpPost(Router.AuthenticationRouting.SignIn)]
        public async Task<ActionResult<JWTAuthResult>> SignInAsync([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid data.", errors = ModelState });

            var user = new EmailAddressAttribute().IsValid(model.EmailOrUserName)
                ? await _userManager.FindByEmailAsync(model.EmailOrUserName)
                : await _userManager.FindByNameAsync(model.EmailOrUserName);

            if (user == null)
                return NotFound(new { mess = "Invalid email or password." });

            if (!user.EmailConfirmed)
                return BadRequest(new { mess = "Email is not confirmed. Please confirm your email." });

            if (user.Inactive)
                return BadRequest(new { mess = "This account is inactive. Please contact support." });

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count == 0)
                return BadRequest(new { mess = "Your request to access this site has been submitted. Please wait until an administrator grants you permission." });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return BadRequest(new { mess = "Invalid email or password." });

            await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, false);

            var accessToken = await _authServices.GetJWTToken(user);

            var authResult = new JWTAuthResult
            {
                AccessToken = accessToken.AccessToken,
                Roles = userRoles.ToList() 
            };

            return Ok(authResult);
        }




        [HttpGet(Router.AuthenticationRouting.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
        {
            if (userId == null || string.IsNullOrEmpty(code))
                return BadRequest(new { mess = "Invalid user ID or confirmation code." });

            var confirmResult = await _authServices.ConfirmEmail(userId, code);

            if (confirmResult == "ErrorWhenConfirmEmail")
                return BadRequest(new { mess = "An error occurred while confirming the email. Please try again." });

            return Redirect("http://localhost:3000/auth/login");
        }


        [HttpPost(Router.AuthenticationRouting.SendCodeResetPassword)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid input data.", errors = ModelState });

            var result = await _authServices.SendResetPasswordCode(resetPassword.Email);

            return result switch
            {
                "notfound" => NotFound(new { mess = "User not found." }),
                "FailedUpdate" or "FailedResetPassword" => BadRequest(new { mess = "An error occurred. Please try again." }),
                "SendEmailSucccess" => Ok(new { mess = "The operation was successful. Please check your email." }),
                _ => BadRequest(new { mess = "An unknown error occurred." })
            };
        }


        [HttpPost(Router.AuthenticationRouting.ConfirmResetPassword)]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ResetPasswordConfirm confirm)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid input data.", errors = ModelState });

            var result = await _authServices.ConfirmResetPassword(confirm.Code, confirm.Email);

            return result switch
            {
                "notfound" => NotFound(new { mess = "User not found." }),
                "failed" => BadRequest(new { mess = "An error occurred. Please try again." }),
                "success" => Ok(new { mess = "The operation was successful." }),
                _ => BadRequest(new { mess = "Invalid code. Please try again." })
            };
        }


        [HttpPost(Router.AuthenticationRouting.ResetPassword)]
        public async Task<IActionResult> ResetPass([FromBody] ResetPass reset)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mess = "Invalid input data.", errors = ModelState });

            var result = await _authServices.ResetPassword(reset.Email, reset.Password);

            return result switch
            {
                "UserNotFound" => BadRequest(new { mess = "User not found." }),
                "Failed" => BadRequest(new { mess = "Invalid code. Please try again." }),
                "Success" => Ok(new { mess = "Password changed successfully." }),
                _ => BadRequest(new { mess = "An unknown error occurred. Please try again." })
            };
        }

    }
}
