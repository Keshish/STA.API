using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STA.API.Dtos.Parent;
using STA.API.Services;
using STA.API.ViewModels;

namespace STA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }

        [Authorize(Roles = "Admin, Supervisor, Assistant")]
        [HttpGet]
        public async Task<IEnumerable<ParentVM>> GetParents()
        {
           
            return await _parentService.GetParentsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateParent(ParentRegisterDto parentRegisterDto)
        {
            var result = await _parentService.RegisterParentAsync(parentRegisterDto);
            if (result)
            {
                return Ok(new { Status = "Success", Message = "Parent created successfully." });
            }
            return BadRequest(new { Status = "Error", Message = "Parent creation failed." });
        }
    }
}
