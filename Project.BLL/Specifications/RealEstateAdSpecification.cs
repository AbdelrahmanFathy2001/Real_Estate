using Microsoft.IdentityModel.Tokens;
using Project.BLL.Attributes;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Specifications
{
    public class RealEstateAdSpecification : BaseSpecification<RealEstateAd>
    {
        public RealEstateAdSpecification(RealEstateAdSpecParams realEstateAdSpecParams)
           : base(p =>
           (string.IsNullOrEmpty(realEstateAdSpecParams.City) || p.City.ToLower().Contains(realEstateAdSpecParams.City)) &&
           (string.IsNullOrEmpty(realEstateAdSpecParams.Region) || p.Region.ToLower().Contains(realEstateAdSpecParams.Region)) &&
           (string.IsNullOrEmpty(realEstateAdSpecParams.Neighborhood) || p.Neighborhood.ToLower().Contains(realEstateAdSpecParams.Neighborhood))&&
           (p.ClientId == realEstateAdSpecParams.ClientId)
           )
        {
            AddInclude(p => p.Client);
            AddInclude(p => p.Images);
            AddInclude(p => p.Client.ApplicationUser);
            AddorderBy(p => p.UnitValue);
            ApplyPagination(realEstateAdSpecParams.PageSize * (realEstateAdSpecParams.PageIndex - 1), realEstateAdSpecParams.PageSize);
        }

        public RealEstateAdSpecification(RealEstateAdPublicSpecParams realEstateAdSpecParams)
          : base(p =>
          (string.IsNullOrEmpty(realEstateAdSpecParams.City) || p.City.ToLower().Contains(realEstateAdSpecParams.City)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.Region) || p.Region.ToLower().Contains(realEstateAdSpecParams.Region)) &&
          (string.IsNullOrEmpty(realEstateAdSpecParams.Neighborhood) || p.Neighborhood.ToLower().Contains(realEstateAdSpecParams.Neighborhood))&&
          (string.IsNullOrEmpty(realEstateAdSpecParams.UnitType) || p.UnitType == EnumTranslations.GetEnumFromTranslation<UnitType>(realEstateAdSpecParams.UnitType))&&
          (string.IsNullOrEmpty(realEstateAdSpecParams.DealType) || p.DealType == EnumTranslations.GetEnumFromTranslation<DealType>(realEstateAdSpecParams.DealType)) &&
          (p.Ispublic == true)

          )
        {
            AddInclude(p => p.Images);
            AddInclude(p => p.Client.ApplicationUser);
            AddorderBy(p => p.UnitValue);
            ApplyPagination(realEstateAdSpecParams.PageSize * (realEstateAdSpecParams.PageIndex - 1), realEstateAdSpecParams.PageSize);
        }

        public RealEstateAdSpecification(int? id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Client);
            AddInclude(p => p.Images);
            AddInclude(p => p.Client.ApplicationUser);
            AddorderBy(p => p.UnitValue);

        }
    }
}
