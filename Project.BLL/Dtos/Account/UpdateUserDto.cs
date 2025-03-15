
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Account
{
    public class UpdateUserDto
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        [Display(Name = "User Name")]

        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
