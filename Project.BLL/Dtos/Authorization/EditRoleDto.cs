using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authorization
{
    public class EditRoleDto
    {
        [Display(Name = "Role Name")]

        public string RoleName { get; set; }
        public string Id { get; set; }
    }
}
