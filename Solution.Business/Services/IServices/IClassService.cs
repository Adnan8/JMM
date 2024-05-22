using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IClassService
    {
        public Task<bool> CreateAsync(ClassVM menu);
        public Task<bool> UpdateAsync(ClassVM menu);
        public Task<bool> DeleteAsync(string id);
        public Task<ClassVM> EditAsync(string id);
        public Task<List<ClassVM>> GetAll();
    }
}
