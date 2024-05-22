using Microsoft.EntityFrameworkCore;
using Solution.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Repository.Repo.IRepo
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenu>
    {
        Task DeleteByRoleId(string roleId);
        Task InsertRange(IEnumerable<RoleMenu> roleMenus);
        void RemoveRange(IEnumerable<RoleMenu> entities);
        Task RemoveRangeAsync(IEnumerable<RoleMenu> entities);
    }
}
