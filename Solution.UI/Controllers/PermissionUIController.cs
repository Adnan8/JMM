using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.UI.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.UI.Controllers
{
    public class PermissionUIController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        public PermissionUIController(IPermissionService permissionService, IRoleService roleService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllPermission(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var permissions = await _permissionService.GetAll();
                var filteredData = permissions.Where(c => string.IsNullOrEmpty(searchValue) || c.PermissionName.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                var paginatedData = filteredData.Skip(start).Take(length).ToList();
                var result = new
                {
                    draw = draw,
                    recordsTotal = permissions.Count,
                    recordsFiltered = filteredData.Count,
                    data = paginatedData
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<PermissionVM>() });
            }
        }

        public async Task<IActionResult> Create()
        {
            var model = new PermissionVM();
            model.Roles = await _roleService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PermissionVM permission)
        {
            if (await _permissionService.CreateAsync(permission))
            {
                TempData["success"] = "Permission created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating permission";
                return RedirectToAction("Error", new { message = "Failed to create permission" });
            }
        }

        public async Task<IActionResult> Edit(string id, string tenantId)
        {
            var permission = await _permissionService.EditAsync(id);
            if (permission != null)
            {
                permission.Roles = await _roleService.GetAll();
                return View(permission);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching permission data" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PermissionVM permission)
        {
            if (await _permissionService.UpdateAsync(permission))
            {
                TempData["success"] = "Permission updated successfully";
                return RedirectToAction("Index","PermissionUI");
            }
            else
            {
                TempData["error"] = "Error updating permission";
                return RedirectToAction("Index", new { message = "Error updating permission" });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var permission = await _permissionService.EditAsync(id);
            if (permission != null)
            {
                return View(permission);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching permission details" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _permissionService.DeleteAsync(id))
            {
                return Json(new { success = true, message = "Permission deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete permission" });
            }
        }

        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            };
            return View(errorViewModel);
        }
    }
}
