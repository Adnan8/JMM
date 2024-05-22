using Microsoft.AspNetCore.Mvc;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.UI.Controllers
{
    public class RolePermissionUIController : Controller
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRoleService _roleService;

        public RolePermissionUIController(IRolePermissionService rolePermissionService, IRoleService roleService)
        {
            _rolePermissionService = rolePermissionService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Roles = await _roleService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RolePermissionVM rolePermission)
        {
            if (await _rolePermissionService.CreateAsync(rolePermission))
            {
                TempData["success"] = "RolePermission added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating RolePermission";
                return RedirectToAction("Error", new { message = "Error creating RolePermission" });
            }
        }

        public async Task<JsonResult> GetPermissionsForRole(string roleId)
        {
            var permissions = await _rolePermissionService.GetPermissionsForRoleAsync(roleId);
            return Json(permissions.Select(x => x.PermissionId).ToList());
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { Message = message });
        }
    }
}
