using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class UserProfileVM
    {
        [Required]
        public string? DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? CompanyName { get; set; }
        public string? RoleId { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? Image { get; set; }
        public string? Path { get; set; }=null;
    }

}
