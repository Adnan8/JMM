using Microsoft.EntityFrameworkCore;
using Solution.DAL.Data;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Repository.Repo
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly AppDbContext _context;

        public RolePermissionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<int>> GetPermissionsForRoleAsync(string roleId)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId).ToListAsync();
            return permissions;
        }
        public async Task DeleteByRoleId(string roleId)
        {
            var itemsToDelete = _context.RolePermissions.Where(rp => rp.RoleId == roleId).ToList();
            _context.RolePermissions.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();
        }
        public async Task InsertRange(IEnumerable<RolePermission> rolePermissions)
        {
            _context.RolePermissions.AddRange(rolePermissions);
            await _context.SaveChangesAsync();
        }
        public void RemoveRange(IEnumerable<RolePermission> entities)
        {
            _context.RolePermissions.RemoveRange(entities);
            _context.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<RolePermission> entities)
        {
            _context.RolePermissions.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
