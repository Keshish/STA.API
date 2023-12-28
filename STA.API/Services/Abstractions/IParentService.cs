using STA.API.Dtos.Parent;
using STA.API.ViewModels;

namespace STA.API.Services.Abstractions
{
    public interface IParentService
    {
        Task<ParentVM> GetParentWithIdAsync(int Id);
        Task<ParentVM> GetParentWithUserIdAsync(string Id);
        Task<List<ParentVM>> GetParentsAsync();
        Task<bool> RegisterParentAsync(ParentRegisterDto parentRegisterDto);
        Task<bool> DeleteParentWithParentIdAsync(int Id);
        Task<bool> UpdateParentAsync(ParentUpdateDto parentUpdateDto);
    }
}
