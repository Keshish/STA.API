using STA.API.Dtos.Course;
using STA.API.Models;
using STA.API.ViewModels;

namespace STA.API.Services.Abstractions
{
    public interface ICourseService
    {
        Task<List<CourseVM>> GetCoursesAsync();
        Task<CourseVM> GetCourseWithIdAsync(int Id);
        Task<bool> RegisterCourseAsync(CourseRegisterDto courseRegisterDto);
        Task<bool> UpdateCourseAsync(CourseUpdateDto courseUpdateDto);
        Task<bool> DeleteCourseWithIdAsync(int Id);
        Task<List<CourseVM>> GetCoursesWithStudentIdAsync(int studentId);
        //Task<IEnumerable<EnrolledCoursesVM>> GetStudentCourseAsync(int studentId);

        Task<IEnumerable<EnrolledCoursesVM>> GetEnrolledCoursesAsync(int studentId);
        Task EnrollStudentInCourseAsync(int studentId, int courseId);
    }
}
