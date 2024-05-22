using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IThemeDetailService
    {
        public Task<bool> CreateAsync(ThemeDetailVM themeDetail);
        public Task<bool> UpdateAsync(ThemeDetailVM themeDetail);
        public Task<bool> DeleteAsync(int id);
        public Task<ThemeDetailVM> EditAsync(int id);
        public Task<ThemeDetailVM> GetByUserId(string id,string CompanyId);
        public Task<List<ThemeDetailVM>> GetAll();
    }
}
