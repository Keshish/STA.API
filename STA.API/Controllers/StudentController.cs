using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STA.API.Dtos.Student;
using STA.API.Models.Users;
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


        [HttpGet("All")]
        public async Task<IEnumerable<StudentVM>> GetStudents()
        {
            var result = await _studentService.GetStudentsAsync();
            return result;
        }
      
        [Authorize(Roles = "Admin, Supervisor, Assistant, Parent")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsWithId(int? parentId = null)
        {
            IEnumerable<StudentVM> result;
            bool isParent = User.IsInRole("Parent");

            if (isParent)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var parent = await _parentService.GetParentWithUserIdAsync(userId);

                if (parent == null)
                {
                    return NotFound("Parent not found.");
                }

                result = await _studentService.GetStudentsWithParentIdAsync(parent.Id);
                return Ok(result);
            }

            result = await _studentService.GetStudentsWithParentIdAsync(parentId ?? -1);

            if (result == null)
            {
                return NotFound("Parent not found.");
            }

            return Ok(result);
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

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(StudentUpdateDto studentUpdateDto)
        {
            var result = await _studentService.UpdateStudentAsync(studentUpdateDto);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Student updated successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Student update failed." });
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            var result = await _studentService.DeleteStudentWithIdAsync(Id);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Student deleted successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Student deletion failed." });
        }

    }
}
