using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class Image:IBaseEntity, IHasPictureUrl
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public int RealEstateAdId { get; set; }

        public RealEstateAd RealEstateAd { get; set; }
    }
}
