using Project.DAL.Entities.Identity;
using Project.DAL.Helper;
using System.IdentityModel.Tokens.Jwt;

namespace Project.BLL.Services
{
    public interface IAuthenticationServices
    {
        public Task<JWTAuthResult> GetJWTToken(ApplicationUser user);
        public JwtSecurityToken ReadJWTToken(string accessToken);
        public Task<string> ValidateToken(string AccessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
        public Task<string> ConfirmEmail(string? userId, string? code);
        public Task<string> SendResetPasswordCode(string Email);
        public Task<string> ConfirmResetPassword(string code, string email);

        public Task<string> ResetPassword(string Email, string Password);

        //public Task<JWTAuthResult> GetRefreshToken(ApplicationUser user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);

    }
}
