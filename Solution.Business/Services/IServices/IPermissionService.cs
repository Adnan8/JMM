using Solution.Common.ViewModel;
using Solution.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IPermissionService
    {
        public Task<bool> CreateAsync(PermissionVM permission);
        public Task<bool> UpdateAsync(PermissionVM permission);
        public Task<bool> DeleteAsync(string id);
        public Task<PermissionVM> EditAsync(string id);
        public Task<List<PermissionVM>> GetAll();
        
    }
}
