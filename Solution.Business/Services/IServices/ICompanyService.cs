using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface ICompanyService
    {
        public Task<int> CreateAsync(CompanyVM companyDto);
        public Task<bool> UpdateAsync(CompanyVM company);
        public Task<bool> DeleteAsync(string id);
        public Task<CompanyVM> EditAsync(string id);
        public Task<List<CompanyVM>> GetAll();
    }
}
