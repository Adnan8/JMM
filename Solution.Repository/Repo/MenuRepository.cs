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
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(AppDbContext context) : base(context)
        { 
        }
       
    }
}
