using AutoMapper;
using STA.API.Dtos.Course;
using STA.API.Models;
using STA.API.ViewModels;

namespace STA.API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            //source -> target
            CreateMap<Course, CourseRegisterDto>();
            CreateMap<CourseRegisterDto, Course>();
            CreateMap<CourseUpdateDto, Course>();
            CreateMap<Course, CourseVM>();
            CreateMap<StudentCourse, EnrolledCoursesVM>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId));

        }
    }
}
