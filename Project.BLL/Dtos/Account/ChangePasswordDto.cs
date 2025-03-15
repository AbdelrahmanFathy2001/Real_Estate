
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Account
{
    public class ChangePasswordDto
    {
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Display(Name = "New Password")]

        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]

        public string ConfirmPassword { get; set; }
    }
}
