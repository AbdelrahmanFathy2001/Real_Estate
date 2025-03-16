using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserRefreshTokens = new HashSet<UserRefreshToken>();
            Subordinates = new HashSet<ApplicationUser>();
        }
        [InverseProperty(nameof(UserRefreshToken.User))]
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? OfficeName { get; set; }

        public string? Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public bool Inactive { get; set; } = false;
        public string? SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }

        // الموظفون التابعون لهذا المستخدم
        [InverseProperty(nameof(Supervisor))]
        public ICollection<ApplicationUser> Subordinates { get; set; }

        public List<ClientInfo> ClientInfos { get; set; }

        public UserImage? Image { get; set; }


    }
}
