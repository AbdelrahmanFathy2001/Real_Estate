
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Account
{
    public class RegisterDto
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        [Display(Name = "User Name")]

        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

    }
}
