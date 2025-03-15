using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos.Office
{
    public class RealEstateAdDto
    {
        public int Id { get; set; }
        public bool Ispublic { get; set; } = true;
        public string Region { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string DealType { get; set; } // Arabic Translation
        public string UnitType { get; set; } // Arabic Translation
        public decimal UnitValue { get; set; }
        public string UnitLocation { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string RequestType { get; set; } // Arabic Translation
        public List<ImageToReturnDto> Images { get; set; }
        public officeDto office { get; set; }
    }
}
