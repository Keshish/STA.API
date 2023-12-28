using STA.API.Dtos.Student;
using STA.API.ViewModels;

namespace STA.API.Services.Abstractions
{
    public interface IStudentService
    {
        Task<List<StudentVM>> GetStudentsWithParentIdAsync(int parentId);
        Task<StudentVM> GetStudentWithIdAsync(int Id);
        Task<bool> RegisterStudenAsync(StudentRegisterDto studentRegisterDto);


    }
}
