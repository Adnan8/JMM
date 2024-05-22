using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class DdlhdrVM
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "DDL name is required")]
        public string? Ddlname { get; set; }
        public string? Ddldesciption { get; set; }
        public Guid? Ddhguid { get; set; }
        public bool? IsSystem { get; set; } = false;
        public int CompId { get; set; }


    }
}
