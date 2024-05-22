using System.ComponentModel.DataAnnotations;

namespace Solution.Common.ViewModel
{
    public class ThemeDetailVM
    {
        public string? Id { get; set; } 

        public string? UserId { get; set; } = null;
        public string? UserName { get; set; } = null;

        public int? CompId { get; set; }
        public string? CompanyName { get; set; }

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

        public bool IsColourMenHeader { get; set; }
        public bool IsDarkMenu { get; set; }

    }
}
