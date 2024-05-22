using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IRolePermissionService
    {
        public Task<bool> CreateAsync(RolePermissionVM rolePermission);
        public Task<bool> UpdateAsync(RolePermissionVM rolePermission);
        public Task<bool> DeleteAsync(int id);
        public Task<RolePermissionVM> EditAsync(int id);
        public Task<List<RolePermissionVM>> GetAll();
        public Task<List<RolePermissionVM>> GetPermissionsForRoleAsync(string roleId);
        Task<List<RoleVM>> GetRolesForPermissionsAsync(int permissionId);
    }
}
