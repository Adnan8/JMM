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
    public partial class Ddlhdr : BaseModel
	{
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("DDLName")]
        [StringLength(50)]
        [Unicode(false)]
        public string Ddlname { get; set; }
        [Required]
        [Column("DDLDesciption")]
        [StringLength(200)]
        [Unicode(false)]
        public string Ddldesciption { get; set; }
        [Column("DDHGuid")]
        public Guid? Ddhguid { get; set; }
        public bool IsSystem { get; set; }

        [InverseProperty("Ddlhdr")]
        public virtual ICollection<DDLDtls> Ddldtls { get; set; }

        //[ForeignKey("CompId")]
        //[InverseProperty("Company")]
        //public virtual Company Company { get; set; }
    }
}

