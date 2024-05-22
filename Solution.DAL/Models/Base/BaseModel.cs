using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DAL.Models.Base
{
	public abstract class BaseModel
	{
		public bool? IsDeleted { get; set; }
		public string? CompId { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; } = DateTime.Now;
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; } = DateTime.Now;

	}
}
