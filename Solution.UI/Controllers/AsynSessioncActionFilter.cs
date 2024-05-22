using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.UI.Controllers
{
    public class AsyncSessionActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var tenantId = context.HttpContext.Session.GetString("TenantId");
            var path = context.HttpContext.Request.Path;

            // Helper method to prepend tenant ID to a given path
            PathString PrependTenantIdToPath(string basePath)
            {
                return string.IsNullOrWhiteSpace(tenantId) ? new PathString(basePath) : new PathString($"/{tenantId}{basePath}");
            }
            if (!string.IsNullOrWhiteSpace(tenantId) && !path.StartsWithSegments(new PathString($"/{tenantId}"), StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Path = PrependTenantIdToPath(path);
            }

            var allowedPathsForAllUsers = new List<string>
            {
                "/AccountUI/Logout",
                "/AccountUI/UserProfile",
                "/AccountUI/Index",
                "/AccountUI/Login",
                "/AccountUI/Register",
                "/AccountUI/ChangePassword"
            };

            // Check if the path is allowed for all users, considering the tenant
            if (allowedPathsForAllUsers.Any(p => path.StartsWithSegments(PrependTenantIdToPath(p), StringComparison.OrdinalIgnoreCase)))
            {
                await next();
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult(PrependTenantIdToPath("/AccountUI/Index")); // Redirect to Login instead of Index if not authenticated
                return;
            }

            List<UserRoleMenusVM> userRolesWithPermissions = new List<UserRoleMenusVM>();
            var rolePermissionsJson = context.HttpContext.Session.GetString("RoleData");

            if (!string.IsNullOrEmpty(rolePermissionsJson))
            {
                userRolesWithPermissions = JsonConvert.DeserializeObject<List<UserRoleMenusVM>>(rolePermissionsJson);
            }

            bool hasEnterpriseRole = userRolesWithPermissions.Any(r => r.RoleId.Equals("Enterprise", StringComparison.OrdinalIgnoreCase));

            bool hasAccess = hasEnterpriseRole; // If the user has the "Enterprise" role, they have access

            if (!hasAccess)
            {
                var permissions = userRolesWithPermissions.SelectMany(rmw => rmw.RolePermissions).ToList();

                Console.WriteLine($"Checking permissions for path: {path}");
                hasAccess = permissions.Any(p =>
                    path.Equals(PrependTenantIdToPath($"/{p.Controller}"), StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWithSegments(PrependTenantIdToPath($"/{p.Controller}/{p.Action}"), StringComparison.OrdinalIgnoreCase) &&
                    (context.ActionDescriptor is ControllerActionDescriptor descriptor &&
                    descriptor.ControllerName.Equals(p.Controller, StringComparison.OrdinalIgnoreCase) &&
                    descriptor.ActionName.Equals(p.Action, StringComparison.OrdinalIgnoreCase)));
            }

            Console.WriteLine($"Requested Path: {path}, Has Access: {hasAccess}");

            if (!hasAccess)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "AccountUI", new { area = "" }); 
            }
            else
            {
                await next();
            }
        }
    }
}
