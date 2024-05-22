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
    public class LoginHistoryRepository : BaseRepository<LoginHistory>, ILoginHistoryRepository
	{
        public LoginHistoryRepository(AppDbContext context) : base(context)
        { 
        }
       
    }
}
