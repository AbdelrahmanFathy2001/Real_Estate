using Project.BLL.Attributes;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Specifications
{
    public class RealEstateAdWithFiltersForCountSpecification: BaseSpecification<RealEstateAd>
    {
        public RealEstateAdWithFiltersForCountSpecification(RealEstateAdSpecParams realEstateAdSpecParams)
           : base(p =>
           (string.IsNullOrEmpty(realEstateAdSpecParams.City) || p.City.ToLower().Contains(realEstateAdSpecParams.City)) &&
           (string.IsNullOrEmpty(realEstateAdSpecParams.Region) || p.Region.ToLower().Contains(realEstateAdSpecParams.Region)) &&
           (string.IsNullOrEmpty(realEstateAdSpecParams.Neighborhood) || p.Neighborhood.ToLower().Contains(realEstateAdSpecParams.Neighborhood)) &&
           (p.ClientId == realEstateAdSpecParams.ClientId)
           )
        {
           
        }

        public RealEstateAdWithFiltersForCountSpecification(RealEstateAdPublicSpecParams realEstateAdSpecParams)
           : base(p =>
          (string.IsNullOrEmpty(realEstateAdSpecParams.City) || p.City.ToLower().Contains(realEstateAdSpecParams.City)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.Region) || p.Region.ToLower().Contains(realEstateAdSpecParams.Region)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.Neighborhood) || p.Neighborhood.ToLower().Contains(realEstateAdSpecParams.Neighborhood)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.UnitType) || p.UnitType == EnumTranslations.GetEnumFromTranslation<UnitType>(realEstateAdSpecParams.UnitType)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.DealType) || p.DealType == EnumTranslations.GetEnumFromTranslation<DealType>(realEstateAdSpecParams.DealType)) &&
          (p.Ispublic == true)
           )
        {

        }
    }
}
