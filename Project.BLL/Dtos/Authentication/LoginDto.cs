
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authentication
{
    public class LoginDto
    {
        [Display(Name = "Email Or UserName")]
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }

    }
}
