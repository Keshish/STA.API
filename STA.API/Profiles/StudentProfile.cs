using AutoMapper;
using STA.API.Dtos.Student;
using STA.API.Models;
using STA.API.ViewModels;

namespace STA.API.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            //source -> target
            CreateMap<Student, StudentVM>();
            CreateMap<StudentRegisterDto, Student>();
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(Student => Student.Id, opt => opt.Ignore()); 
        }
    }
}
