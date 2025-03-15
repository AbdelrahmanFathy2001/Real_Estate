using AutoMapper;
using Project.BLL.Attributes;
using Project.BLL.Dtos.Office;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;
using Project.BLL.Specifications;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project.BLL.Services
{
    public class RealEstateAdService : IRealEstateAdService
    {
        private readonly IGenericRepository<RealEstateAd> _repository;
        private readonly IMapper _mapper;

        public RealEstateAdService(IGenericRepository<RealEstateAd> repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RealEstateAdDto>> GetAllAsync()
        {
            var ads = await _repository.GetAllAsync();
            return ads.Select(ad => ConvertToDto(ad));
        }

        public async Task<RealEstateAdDto> GetByIdAsync(int id)
        {
            var ad = await _repository.GetByIdAsync(id);
            return ad != null ? ConvertToDto(ad) : null;
        }

        public async Task AddAsync(RealEstateAd ad)
        {
            await _repository.AddAsync(ad);
        }

        public async Task UpdateAsync(RealEstateAd ad)
        {
            await _repository.UpdateAsync(ad);
        }

        public async Task DeleteAsync(int id)
        {
            var ad = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(ad);
        }

        private RealEstateAdDto ConvertToDto(RealEstateAd ad)
        {
            var apiUrl = "https://localhost:7276/images/products/"; // الحصول على رابط API الأساسي

            return new RealEstateAdDto
            {
                Id = ad.Id,
                Region = ad.Region,
                City = ad.City,
                Neighborhood = ad.Neighborhood,
                DealType = EnumTranslations.GetTranslation(ad.DealType),
                UnitType = EnumTranslations.GetTranslation(ad.UnitType),
                UnitValue = ad.UnitValue,
                UnitLocation = ad.UnitLocation,
                Longitude = ad.Longitude,
                Latitude = ad.Latetude,
                RequestType = EnumTranslations.GetTranslation(ad.RequestType),

                Images = ad.Images?.Select(img => new ImageToReturnDto
                {
                    Id = img.Id,
                    Name = new Uri(new Uri(apiUrl), img.PictureUrl).ToString() // تكوين رابط كامل للصور
                }).ToList() ?? new List<ImageToReturnDto>(),

                office = new officeDto
                {
                    Id = ad.Client.ApplicationUserId,
                    UserName = ad.Client.ApplicationUser.UserName,
                }
            };
        }



        public async Task<RealEstateAdDto> GetByIdWithSpecAsync(ISpecification<RealEstateAd> spec)
        {
            var ad = await _repository.GetByIdWithSpecAsync(spec);
            return ad != null ? ConvertToDto(ad) : null;
        }

        public async Task<IReadOnlyList<RealEstateAdDto>> GetAllWithSpecAsync(ISpecification<RealEstateAd> spec)
        {
            var ads = await _repository.GetAllWithSpecAsync(spec); 
            return ads.Select(ad => ConvertToDto(ad)).ToList(); 
        }

    }
}
