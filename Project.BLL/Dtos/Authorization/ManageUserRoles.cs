using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authorization
{

    public class ManageUserRoles
    {
        public string UserId { get; set; }
        [Display(Name = "User Name")]
        public string username { get; set; }
        public List<Roles> Roles { get; set; }
        public List<string> SelectedRoleIds { get; set; } // Or List<string> if RoleId is string

    }

    public class ManageUsers
    {
        public string UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
        public List<Roles> Roles { get; set; }

    }

    public class Roles
    {
        public string RoleId { get; set; }
        [Display(Name = "Role Name")]

        public string RoleName { get; set; }

        public bool HasRole { get; set; }
    }
}