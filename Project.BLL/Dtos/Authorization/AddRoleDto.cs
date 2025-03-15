using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authorization
{
    public class AddRoleDto
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

    }
}
