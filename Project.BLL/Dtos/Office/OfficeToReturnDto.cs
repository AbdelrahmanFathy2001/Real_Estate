using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos.Office
{
    public class OfficeToReturnDto
    {
        public string Id { get; set; }
        [Display(Name = "Office Name")]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        public List<RealEstateAdDto> RealEstateAds { get; set; }

    }
}
