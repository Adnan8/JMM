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
    public class StudentClass : BaseModel
    {
        public int StudentClassId { get; set; }
        public string UserId { get; set; }
        public int ClassId { get; set; }
		[InverseProperty("StudentClass")]
		[ForeignKey("ClassId")]
		public Class Class { get; set; }
    }

}
