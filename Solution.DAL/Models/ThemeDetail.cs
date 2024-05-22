using Solution.DAL.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.DAL.Models
{
    public partial class ThemeDetail : BaseModel
	{
		[Key]
		public int Id { get; set; }

		public string? UserId { get; set; }

        public int? LogoId { get; set; }

        [StringLength(255)]
		public string Primarybg { get; set; }

		[StringLength(255)]
		public string Primaryfg { get; set; }

		[StringLength(255)]
		public string Secondarybg { get; set; }

		[StringLength(255)]
		public string Secondaryfg { get; set; }

		[StringLength(255)]
		public string Tertiarybg { get; set; }

		[StringLength(255)]
		public string Tertiaryfg { get; set; }

		public bool? IsColourMenHeader { get; set; }

		public bool? IsDarkMenu { get; set; }
		
	}
}
