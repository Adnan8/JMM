using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
	public partial class RoleMenuVM
	{
	
		public string? Id { get; set; }

		public string? MenuId { get; set; } 
        public List<string> RoleMenuId { get; set; }


		public string RoleId { get; set; } = null!;

    }
}
