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
    public class Company : BaseModel
	{
        [Key]
        public int Id { get; set; }
        public string Compname { get; set; } = null!;
        public string? Abn { get; set; }
        public string? Addr1 { get; set; }
        public string? Addr2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pcode { get; set; }
        public string? Country { get; set; }
        public string? Webaddr { get; set; }
        public int? Invoicedays { get; set; }
        public string? Invoiceterms { get; set; }
        public string? Logo { get; set; }
        public int SecModel { get; set; }
        public Guid? CompGuid { get; set; }
        public string? URCode { get; set; }
        public bool? DefaultPermissions { get; set; }

		[InverseProperty("Companies")]
		public virtual ICollection<Student> Student { get; set; }
	}
}
