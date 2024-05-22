using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solution.DAL.Models.Base;

namespace Solution.DAL.Models
{
    public partial class DDLDtls : BaseModel
	{
        [Key]
        public int Id { get; set; }
        [Column("DDLHdrId")]
        public int DdlhdrId { get; set; }
        [Required]
        [Column("DDLTxt")]
        [StringLength(100)]
        [Unicode(false)]
        public string Ddltxt { get; set; }
        [Column("DDLOrder")]
        public int Ddlorder { get; set; }
        [Column("DDlGuid")]
        public Guid? DdlGuid { get; set; }
        public int? Data1 { get; set; }
        public int? Data2 { get; set; }

        [ForeignKey("DdlhdrId")]
        [InverseProperty("Ddldtls")]
        public virtual Ddlhdr Ddlhdr { get; set; }


    }
}
