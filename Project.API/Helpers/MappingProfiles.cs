using Project.BLL.Dtos;
using Project.BLL.Dtos.Office;

namespace Project.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Image, ImageToReturnDto>()
                .ReverseMap();

            CreateMap<ClientInfo, clientDto>()
                .ReverseMap();
            CreateMap<RealEstateAd, RealEstateAdDto>()
                 .ForMember(d => d.Images, opt => opt.MapFrom<ProductImagesUrlResolver<RealEstateAd, RealEstateAdDto>>())
                .ReverseMap();



        }
    }
}
