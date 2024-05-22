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
    public class DocumentRepository:BaseRepository<Document>,IDocumentRepository
    {
        public DocumentRepository(AppDbContext context) : base(context)
        { 
        }
       
    }
}
