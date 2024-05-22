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
    public class DDLDtlsRepository : BaseRepository<DDLDtls>, IDDLDtlsRepository
    {
        public DDLDtlsRepository(AppDbContext context) : base(context)
        {

        }
       
    }
}
