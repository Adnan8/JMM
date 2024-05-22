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
    public class RoleMenuRepository : BaseRepository<RoleMenu>, IRoleMenuRepository
    {
        private readonly AppDbContext _context;

        public RoleMenuRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task DeleteByRoleId(string roleId)
        {
            var itemsToDelete = _context.RoleMenu.Where(rp => rp.RoleId == roleId).ToList();
            _context.RoleMenu.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();
        }
        public async Task InsertRange(IEnumerable<RoleMenu> roleMenus)
        {
            _context.RoleMenu.AddRange(roleMenus);
            await _context.SaveChangesAsync();
        }
        public void RemoveRange(IEnumerable<RoleMenu> entities)
        {
            _context.RoleMenu.RemoveRange(entities);
            _context.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<RoleMenu> entities)
        {
            _context.RoleMenu.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

    }
}
