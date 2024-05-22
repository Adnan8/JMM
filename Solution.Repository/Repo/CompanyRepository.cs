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
    public class CompanyRepository:BaseRepository<Company>,ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context)
        { 
        }
       
    }
}
