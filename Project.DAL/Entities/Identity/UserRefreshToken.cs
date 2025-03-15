using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DAL.Entities.Identity
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string? JwtId { get; set; }

        public UserRefreshToken()
        {
            IsActive = DateTime.Now > ExpiredAt ? false : true;
        }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(ApplicationUser.UserRefreshTokens))]
        public ApplicationUser? User { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsActive { get; set; }

        public DateTime AddTime { get; set; }
        public DateTime ExpiredAt { get; set; }


    }
}
