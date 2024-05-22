using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IUserContextService
    {
        string GetUserId();
        string GetCompanyId();
        Task<(string RoleId, string RoleName)> GetRoleInfoAsync();
    }

}
