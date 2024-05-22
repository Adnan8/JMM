using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.Common.ViewModel
{
    public partial class RolePermissionVM
    {

        public string? Id { get; set; }
        public string? PermissionId { get; set; } 

        public List<string> RolePermissionId { get; set; }
        public string? RoleId { get; set; }


    }
}




