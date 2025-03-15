

using Project.BLL.Dtos.Office;

namespace Project.API.Mapping
{
    public partial class AccountMapping
    {

        public void GetAllUsers()
        {
            CreateMap<ApplicationUser, GetAllUsersDto>().
                ForMember(des => des.Phone, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap(); ;
            CreateMap<ApplicationUser, OfficeToReturnDto>()
                .ForMember(des => des.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();

        }
    }
}
