using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Common.ViewModel
{
    public class MenuVM
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        public string? Descr { get; set; }

        public int? ParentId { get; set; }

        public string? Icon { get; set; }
        [Required(ErrorMessage = "Url is required")]
        public string? Url { get; set; }

        public int? MenuOrder { get; set; }
        [Required(ErrorMessage = "Controller is required")]
        public string? Controller { get; set; }
        [Required(ErrorMessage = "Page is required")]
        public string? Page { get; set; }
        public bool? IsDefault { get; set; }
        public string? MenuLevel { get; set; }

        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
        public List<SelectListItem> ParentMenus { get; set; } = new List<SelectListItem>();
        public List<MenuVM> Children { get; set; } = new List<MenuVM>();
    }
}
