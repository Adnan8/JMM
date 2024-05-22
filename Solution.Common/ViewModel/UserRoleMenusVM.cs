using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class UserRoleMenusVM
    {
        public string RoleId { get; set; } 
        public string RoleName { get; set; } 
        public List<MenuVM> RoleMenus { get; set; } = new List<MenuVM>();
        public List<PermissionVM> RolePermissions { get; set; } = new List<PermissionVM>(); 
    }

}
