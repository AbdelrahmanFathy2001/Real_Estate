using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class RealEstateAd: IHasListImages
    {
        public int Id { get; set; }

        public bool Ispublic { get; set; } = true;

        public string Description { get; set; }

        // المنطقه
        [Required]
        public string Region { get; set; }
        // المدينه
        [Required]
        public string City { get; set; }
        // الحي
        public string Neighborhood { get; set; }

        public string UnitLocation { get; set; }

        public double Longitude { get; set; } 

        public double Latetude { get; set; } 

        public decimal UnitValue { get; set; }

        public DealType DealType { get; set; }

        public UnitType UnitType { get; set; }

        public RequestType RequestType { get; set; }

        public List<Image> Images { get; set; }

        public int? ClientId { get; set; }

        public ClientInfo? Client { get; set; }

    }
}
