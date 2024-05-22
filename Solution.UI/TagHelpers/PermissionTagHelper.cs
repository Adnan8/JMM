

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Solution.Common.ViewModel;
using System.Security.Claims;

namespace Solution.UI.TagHelpers
{

    [HtmlTargetElement(Attributes = "asp-permission")]
    public class PermissionTagHelper : TagHelper
    {
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionTagHelper(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;

        }


        [HtmlAttributeName("asp-permission")]
        public Permissions Permission { get; set; }

        // This is how you inject ViewContext into a TagHelper
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var userId = ViewContext.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var user = _httpContextAccessor.HttpContext.User;
            var isAuthenticated = user.Identity.IsAuthenticated; // Should now be true
            //var userId = user.FindFirst("UserId")?.Value;
            var userId = user.Claims.First().Value;


            var currentPagePath = ViewContext.HttpContext.Request.Path;
            var currentPage = ViewContext.HttpContext.Request.Path.ToString();
            var rolePermissionsJson = _httpContextAccessor.HttpContext.Session.GetString("RoleData");
            List<UserRoleMenusVM> userRolesWithPermissions = new List<UserRoleMenusVM>();

            if (!string.IsNullOrEmpty(rolePermissionsJson))
            {
                userRolesWithPermissions = JsonConvert.DeserializeObject<List<UserRoleMenusVM>>(rolePermissionsJson);
            }
            if (userId == null || !_permissionService.HasPermission(userId, Permission, currentPage, userRolesWithPermissions))
            {
                output.SuppressOutput();
            }
        }
    }
    public interface IPermissionService
    {
        bool HasPermission(string userId, Permissions permission, string currentPage, List<UserRoleMenusVM> userRolesWithPermissions);
    }

    public class PermissionService : IPermissionService
    {
        public bool HasPermission(string userId, Permissions permission, string currentPage, List<UserRoleMenusVM> userRolesWithPermissions)
        {
            if (userRolesWithPermissions.Any(x => x.RoleId != "Enterprise"))
            {
                var permissions = userRolesWithPermissions
                                    .Where(x => x.RoleId != "Enterprise")
                                    .SelectMany(rmw => rmw.RolePermissions)
                                    .ToList();

                string actionName = Enum.GetName(typeof(Permissions), permission);

				bool hasAccess = permissions.Any(p =>
		   currentPage.Equals($"/{p.Controller}", StringComparison.OrdinalIgnoreCase) &&
		   (actionName.Equals("Index", StringComparison.OrdinalIgnoreCase) || actionName.Equals(p.Action, StringComparison.OrdinalIgnoreCase)) ||
		   currentPage.Equals($"/{p.Controller}/{p.Action}", StringComparison.OrdinalIgnoreCase));

				if (!hasAccess)
                {
                    return false;
                }
            }

            return true;
        }
    }
    public enum Permissions
    {
        Index,
        Create,
        Edit,
        Delete,
        Details
    }
}
