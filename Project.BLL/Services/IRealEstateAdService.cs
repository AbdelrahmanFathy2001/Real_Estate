using Project.BLL.Dtos.Office;
using Project.BLL.Specifications;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public interface IRealEstateAdService
    {
        Task<IEnumerable<RealEstateAdDto>> GetAllAsync();
        Task<RealEstateAdDto> GetByIdAsync(int id);
        Task<RealEstateAdDto> GetByIdWithSpecAsync(ISpecification<RealEstateAd> spec);
        Task<IReadOnlyList<RealEstateAdDto>> GetAllWithSpecAsync(ISpecification<RealEstateAd> spec);
        Task AddAsync(RealEstateAd ad);
        Task UpdateAsync(RealEstateAd ad);
        Task DeleteAsync(int id);
    }
}
