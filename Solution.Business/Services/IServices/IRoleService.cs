using Solution.Common.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IRoleService
    {
        Task<bool> CreateAsync(RoleVM roleDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(RoleVM roleDto);
        Task<List<RoleVM>> GetAll();
        Task<RoleVM> EditAsync(string id);
    }
}
