using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solution.Repository;
using System.Security;
using System.Net;
using System.Security.Cryptography;
using Solution.DAL.Models;
using Solution.DAL.Data;

namespace Solution.Repository.Repo
{
    public class ThemeDetailRepository : BaseRepository<ThemeDetail>, IThemeDetailRepository
    {
        public ThemeDetailRepository(AppDbContext context) : base(context)
        {
        }
    }

}
