using Solution.Common.ViewModel;
using Solution.DAL.Data;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;

public interface IRolePermissionRepository : IBaseRepository<RolePermission>
{
    Task<List<int>> GetPermissionsForRoleAsync(string roleId);
    Task DeleteByRoleId(string roleId);
    Task InsertRange(IEnumerable<RolePermission> rolePermissions);
    void RemoveRange(IEnumerable<RolePermission> entities);
    Task RemoveRangeAsync(IEnumerable<RolePermission> entities);
}

