using STA.API.Dtos.Parent;
using STA.API.ViewModels;

namespace STA.API.Services
{
    public interface IParentService
    {
        Task<ParentVM> GetParentAsync(int Id);
        Task<List<ParentVM>> GetParentsAsync();
        Task<bool> RegisterParentAsync(ParentRegisterDto parentRegisterDto);

    }
}
