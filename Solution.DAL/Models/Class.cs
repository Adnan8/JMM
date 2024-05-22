using Solution.Business.Enums;
using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DAL.Models
{
    public class Class : BaseModel
    {
        public int ClassId { get; set; }
        public string GradeLevel { get; set; }
        public string ProgramDetails { get; set; }
        public ClassTimeSlots? ClassTime { get; set; }
        public int MaxSize { get; set; }
		[InverseProperty("Class")]
		public List<StudentClass> StudentClass { get; set; }
    }

}
