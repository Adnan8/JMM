using Solution.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class LoginResponseVM
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? CompanyId { get; set; }
        public string? Token { get; set; }
        public IEnumerable<UserRoleMenusVM>? RoleData { get; set; }
        public LoginResult LoginResult { get; set; }
    }

}
