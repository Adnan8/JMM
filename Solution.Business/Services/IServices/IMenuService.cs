using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IMenuService
    {
        public Task<bool> CreateAsync(MenuVM menu);
        public Task<bool> UpdateAsync(MenuVM menu);
        public Task<bool> DeleteAsync(string id);
        public Task<MenuVM> EditAsync(string id);
        public Task<List<MenuVM>> GetAll();
    }
}
