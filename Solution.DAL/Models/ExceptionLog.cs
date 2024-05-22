using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DAL.Models
{
	public class ExceptionLog : BaseModel
	{
		[Key]
		public int Id { get; set; }
		public string? ControllerName { get; set; }
		public string? ActionName { get; set; }
		public string? ExceptionMessage { get; set; }
		public string? StackTrace { get; set; }
		public DateTime? Timestamp { get; set; } = DateTime.UtcNow;
	}
}
