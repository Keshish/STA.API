using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STA.API.Dtos.Student;
using STA.API.Services.Abstractions;
using STA.API.ViewModels;
using System.Security.Claims;

namespace STA.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IParentService _parentService;
        public StudentController(IStudentService sudentService, IParentService parentService)
        {
            _studentService = sudentService;
            _parentService = parentService;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Supervisor, Assistant, Parent")]
        [HttpGet]
        public async Task<IEnumerable<StudentVM>> GetStudents(int? parentId = null)
        {
            bool isParent = User.IsInRole("Parent");
            if (isParent)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var parent = await _parentService.GetParentWithUserIdAsync(userId);
                return await _studentService.GetStudentsWithParentIdAsync(parent.Id);
            }

            return await _studentService.GetStudentsWithParentIdAsync(parentId?? -1);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentRegisterDto studentRegisterDto)
        {
            var result = await _studentService.RegisterStudenAsync(studentRegisterDto);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Student created successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Student creation failed." });
        }


    }
}
