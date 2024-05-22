using Solution.Common.ViewModel;
using Solution.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IDDLDtlsService
    {
       public Task<bool> CreateAsync(DDLDtlsVM dDLDtls);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> UpdateAsync( DDLDtlsVM updatedDtlsDto);
        public Task<DDLDtlsVM> EditAsync(int id);
        public Task<List<DDLDtlsVM>> GetAll();

    }
}
