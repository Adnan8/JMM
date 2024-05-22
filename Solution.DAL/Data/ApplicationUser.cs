using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Solution.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? SecurityKey { get; set; }
        public string? UserType { get; set; }
        public int? PortalId { get; set; }
		public int? CompId { get; set; }
        public bool? IsCompany { get; set; }
        public DateTime? ActivationDate { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ProfileImage { get; set; }
	}
}