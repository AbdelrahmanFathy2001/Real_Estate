using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.API.Helpers.Account;
using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;
using Project.DAL.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.BLL.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailServices _emailServices;
        private readonly ApplicationDbContext _context;


        public AuthenticationServices(JwtSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository,
            UserManager<ApplicationUser> userManager, IEmailServices emailServices, ApplicationDbContext context
)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _emailServices = emailServices;
            _context = context;
        }
        public async Task<JWTAuthResult> GetJWTToken(ApplicationUser user)
        {

            var (jwtToken, accessToken) = await GenerateJWtToken(user);
            // Get Refresh Token
            var refreshtoken = GetRefreshToken(user.UserName);
            if (refreshtoken == null) throw new InvalidOperationException("Failed to generate refresh token.");

            var userRefreshtoken = new UserRefreshToken()
            {
                AddTime = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                Token = accessToken,
                IsRevoked = false,
                UserId = user.Id,
                JwtId = jwtToken.Id,
                RefreshToken = refreshtoken.TokenString,
            };

            await _refreshTokenRepository.AddAsync(userRefreshtoken);

            var response = new JWTAuthResult
            {
                RefreshToken = refreshtoken,
                AccessToken = accessToken
            };

            return response;

        }



        //helper methods

        private async Task<(JwtSecurityToken, string)> GenerateJWtToken(ApplicationUser user)
        {

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (_jwtSettings == null) throw new InvalidOperationException("JWT settings cannot be null.");
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret)) throw new InvalidOperationException("JWT secret cannot be null or empty.");
            var roles = await _userManager.GetRolesAsync(user);
            // Get Claims
            var claims = GetClaims(user, roles.ToList());
            if (claims == null || !claims.Any()) throw new InvalidOperationException("No claims found for the user.");

            // Generate Token
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));

            // Token 
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            return (jwtToken, accessToken);
        }


        private RefreshToken GetRefreshToken(string username)
        {

            var refreshtoken = new RefreshToken()
            {
                UserName = username,
                ExpireAt = DateTime.Now.AddYears(_jwtSettings.RefreshTokenExpireDate),
                TokenString = GenerateRefreshToken()
            };

            return refreshtoken;
        }
        private string GenerateRefreshToken()
        {

            var randomNumber = new Byte[32];
            var randomnumberGenerate = RandomNumberGenerator.Create();
            randomnumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public List<Claim> GetClaims(ApplicationUser user, List<string> roles)
        {
            List<Claim> claims = new List<Claim>()
            {

                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.MobilePhone , user.PhoneNumber),
                new Claim(ClaimTypes.Country , user.Country),
                new Claim(nameof(UserClaimModel.City), user.City),
                new Claim(nameof(UserClaimModel.Street), user.Street),
                new Claim(nameof(UserClaimModel.Id), user.Id),

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;

        }


        //public async Task<JWTAuthResult> GetRefreshToken(ApplicationUser user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        //{
        //    var (, newToken) = await GenerateJWtToken(user);
        //    var response = new JWTAuthResult();
        //    response.AccessToken = newToken;
        //    var refreshTokenResult = new RefreshToken();
        //    refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName)).Value;
        //    refreshTokenResult.TokenString = refreshToken;
        //    refreshTokenResult.ExpireAt = (DateTime)expiryDate;
        //    response.RefreshToken = refreshTokenResult;
        //    return response;

        //}

        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                if (validator == null)
                {
                    return "InvalidToken";
                }

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("AlgorithmIsWrong", null);
            }
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("TokenIsNotExpired", null);
            }

            //Get User

            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                             .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                     x.RefreshToken == refreshToken &&
                                                                     x.UserId == userId);
            if (userRefreshToken == null)
            {
                return ("RefreshTokenIsNotFound", null);
            }

            if (userRefreshToken.ExpiredAt < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsActive = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }
            var expirydate = userRefreshToken.ExpiredAt;
            return (userId, expirydate);
        }



        ///////////////////////////////////////////////////////////////
        ///


        public async Task<string> ConfirmEmail(string? userId, string? code)
        {
            if (userId.IsNullOrEmpty() || code.IsNullOrEmpty())
                return "ErrorWhenConfirmEmail";
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var confirmEmail = await _userManager.ConfirmEmailAsync(user, code);
            if (!confirmEmail.Succeeded)
                return "ErrorWhenConfirmEmail";
            return "Success";
        }

        public async Task<string> SendResetPasswordCode(string Email)
        {
            //1-User
            //2-not Founded
            //3-Geneate Random number Code 
            //4-Update user Code In dB
            //5-Send COdeto Email 
            //6-success
            var trans = _context.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null) return "notfound";
                Random generator = new Random();
                string RandomCode = generator.Next(0, 10000000).ToString("D6");
                user.Code = RandomCode;
                var UpdateResult = await _userManager.UpdateAsync(user);
                if (!UpdateResult.Succeeded) return "FailedUpdate";
                var mess = $"Code To Reset Password <b class='text-primary'>{RandomCode}<b>";

                await _emailServices.SendEmailAsync(user.Email, mess, "Reset Password");

                await trans.CommitAsync();
                return "SendEmailSucccess";


            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "FailedResetPassword";
            }




        }

        public async Task<string> ConfirmResetPassword(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "notfound";
            if (user.Code == code)
                return "success";
            return "failed";
        }

        public async Task<string> ResetPassword(string Email, string Password)
        {
            var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                    return "UserNotFound";
                await _userManager.RemovePasswordAsync(user);
                if (!await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.AddPasswordAsync(user, Password);
                }
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
    }
}