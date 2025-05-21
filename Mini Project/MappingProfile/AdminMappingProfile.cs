using AutoMapper;
using Mini_Project.Data;
using Mini_Project.Dtos;
using Mini_Project.Models;

namespace Mini_Project.MappingProfile
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<AppUser, GetAdminDto>()
                .ForMember(dest => dest.Username, source => source.MapFrom(source => source.UserName));
        }
    }
}
