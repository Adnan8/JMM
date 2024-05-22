using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public partial class RoleVM()
    {

        public string? Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string? Name { get; set; }

        public string? NormalizedName { get; set; }

        public string? ConcurrencyStamp { get; set; }
        public bool? IsSelected { get; set; }
    }
   
}

