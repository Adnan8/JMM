using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IDdlhdrService
    {
        public Task<bool> CreateAsync(DdlhdrVM ddlhdrVM);
        public Task<bool> UpdateAsync( DdlhdrVM dDlhdrDto);
        public Task<bool> DeleteAsync(string id);
        public Task<DdlhdrVM> EditAsync(string id);
        public Task<List<DdlhdrVM>> GetAll();
        Task<List<DDLDtlsVM>> GetDdldtlsByDdlhdrIdAsync(string ddlhdrId);

    }
}
