using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IRoleMenuService
    {
        public Task<bool> CreateAsync(RoleMenuVM roleMenu);
        public Task<bool> UpdateAsync(RoleMenuVM roleMenu);
        public Task<bool> DeleteAsync(int id);
        public Task<RoleMenuVM> EditAsync(int id);
        public Task<List<RoleMenuVM>> GetAll();
        public Task<List<RoleMenuVM>> GetMenusForRoleAsync(string roleId);
        Task<List<RoleVM>> GetRolesForMenuAsync(int menuId);
    }
}
