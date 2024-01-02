using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STA.API.Dtos.Course;
using STA.API.Models;
using STA.API.Models.DbContext;
using STA.API.Services.Abstractions;
using STA.API.ViewModels;

namespace STA.API.Services.Implementations
{

    public class CourseService : ICourseService
    {
        private readonly BaseDataContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<CourseService> _logger;

        public CourseService(BaseDataContext dbContext, ILogger<CourseService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<CourseVM>> GetCoursesAsync()
        {
            _logger.LogInformation("GetCoursesAsync called.");
            var courses = await _dbContext.Courses.ToListAsync();

            return _mapper.Map<List<CourseVM>>(courses);
        }

        public async Task<CourseVM> GetCourseWithIdAsync(int Id)
        {
            _logger.LogInformation("GetCourseWithIdAsync called.");
            var course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == Id);

            if (course == null)
            {
                throw new ApplicationException("Course not found.");
            }
            return _mapper.Map<CourseVM>(course);
        }

        public async Task<bool> RegisterCourseAsync(CourseRegisterDto courseRegisterDto)
        {
            _logger.LogInformation("RegisterCourseAsync called.");

            var course = _mapper.Map<Course>(courseRegisterDto);

            _ = _dbContext.Courses.Add(course);
            _ = _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateDto courseUpdateDto)
        {
            _logger.LogInformation("UpdateCourseAsync called.");

            var course = _mapper.Map<Course>(courseUpdateDto);

            _ = _dbContext.Courses.Update(course);
            _ = _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteCourseWithIdAsync(int Id)
        {
            _logger.LogInformation("DeleteCourseWithIdAsync called.");

            var course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == Id);
            if (course == null)
            {
                throw new ApplicationException("Course not found.");
            }
            _ = _dbContext.Courses.Remove(course);
            _ = await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CourseVM>> GetCoursesWithStudentIdAsync(int studentId)
        {
            _logger.LogInformation("GetCoursesWithStudentIdAsync called.");
            var courses = await _dbContext.Courses.Where(x => x.Id == studentId).ToListAsync();

            return _mapper.Map<List<CourseVM>>(courses);
        }


        public async Task EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            _logger.LogInformation("EnrollStudentInCourseAsync called.");
            var student = await _dbContext.Students.FindAsync(studentId);
            var course = await _dbContext.Courses.FindAsync(courseId);

            if (student == null || course == null)
            {
                return;
            }

            var studentCourse = new StudentCourse { Student = student, Course = course };

            student.StudentCourses.Add(studentCourse);
            course.StudentCourses.Add(studentCourse);

            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EnrolledCoursesVM>> GetEnrolledCoursesAsync(int studentId)
        {
            _logger.LogInformation("GetEnrolledCoursesAsync called.");
            var student = await _dbContext.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return Enumerable.Empty<EnrolledCoursesVM>();
            }

            var enrolledCourses = _mapper.Map<IEnumerable<EnrolledCoursesVM>>(student.StudentCourses);

            return enrolledCourses;
        }
    }
}
