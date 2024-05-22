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
    public class StudentRepository:BaseRepository<Student>,IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        { 
        }
       
    }
}
