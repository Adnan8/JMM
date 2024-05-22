using Solution.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DAL.Models
{
	public class LoginHistory 
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime LoginTime { get; set; }
		public string IPAddress { get; set; }
	}

}
