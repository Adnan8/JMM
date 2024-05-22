using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DAL.Models
{
	public class AcademicYear
	{
		public int AcademicYearId { get; set; }
		public string? Year { get; set; }
		public string? MonthName { get; set; }
		public DateTime? SessionStart { get; set; }
		public DateTime? SessionEnd { get; set; }
	}
}
