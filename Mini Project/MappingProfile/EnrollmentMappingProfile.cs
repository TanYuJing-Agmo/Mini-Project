using AutoMapper;
using Mini_Project.Dtos;
using Mini_Project.Models;

namespace Mini_Project.MappingProfile
{
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.UserName, source => source.MapFrom(source => source.Student.UserName))
                .ForMember(dest => dest.CourseName, source => source.MapFrom(source => source.Course.Name));
        }
    }
}
