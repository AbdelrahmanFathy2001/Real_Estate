using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos.Office
{
    public class officeDto
    {
        public string Id { get; set; }
        [Display(Name = "Office Name")]
        public string UserName { get; set; }
    }
}
