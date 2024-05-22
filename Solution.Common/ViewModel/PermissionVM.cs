using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solution.Common.ViewModel
{
    public partial class PermissionVM
    {

        public string? Id { get; set; }
        [Required(ErrorMessage = "PermissionName is required")]

        public string? PermissionName { get; set; }
        [Required(ErrorMessage = "Controller is required")]

        public string? Controller { get; set; }
        [Required(ErrorMessage = "Action is required")]

        public string? Action { get; set; }

        public int? OperationType { get; set; }
        public bool? IsDefault { get; set; }
        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }

}




