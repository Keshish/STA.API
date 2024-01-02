using Microsoft.AspNetCore.Mvc;
using STA.API.Dtos.Course;
using STA.API.Services.Abstractions;
using STA.API.Services.Implementations;
using STA.API.ViewModels;

namespace STA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var result = await _courseService.GetCoursesAsync();
            if (result == null)
            {
                return NotFound("Courses not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseRegisterDto courseRegisterDto)
        {
            var result = await _courseService.RegisterCourseAsync(courseRegisterDto);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Course created successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Course creation failed." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseUpdateDto courseUpdateDto)
        {
            var result = await _courseService.UpdateCourseAsync(courseUpdateDto);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Course updated successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Course update failed." });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            var result = await _courseService.DeleteCourseWithIdAsync(Id);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Course deleted successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Course deletion failed." });
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetCoursesWithStudentId(int studentId)
        {
            var result = await _courseService.GetCoursesWithStudentIdAsync(studentId);
            if (result == null)
            {
                return NotFound("Courses not found.");
            }
            return Ok(result);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            await _courseService.EnrollStudentInCourseAsync(studentId, courseId);

            return Ok();
        } 

        [HttpGet("enrolled-courses")]
        public async Task<IActionResult> DisplayEnrolledCoursesAsync(int studentId)
        {
            var enrolledCourses = await _courseService.GetEnrolledCoursesAsync(studentId);

            return Ok(enrolledCourses); // Return an appropriate response
        }

    }
}
