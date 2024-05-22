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
    public class DdlhdrRepository : BaseRepository<Ddlhdr>, IDdlhdrRepository
    {
        private readonly AppDbContext _context;
        public DdlhdrRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        //public async Task<List<int>> GetDdldtlsForDdlhdrAsync(string ddlhdrId)
        //{
        //    var ddldtls = await _context.DDLDtls
        //        .Where(dd => dd.DdlhdrId == ddlhdrId)
        //        .Select(dd => dd.DdldtlsId)
        //        .ToListAsync();

        //    return ddldtls;
        //}

    }
}
