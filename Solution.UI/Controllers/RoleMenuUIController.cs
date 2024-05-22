using Microsoft.AspNetCore.Mvc;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.UI.Models;
using System;
using System.Threading.Tasks;

namespace Solution.UI.Controllers
{
    public class RoleMenuUIController : Controller
    {
        private readonly IRoleMenuService _roleMenuService;
        private readonly IRoleService _roleService;

        public RoleMenuUIController(IRoleMenuService roleMenuService, IRoleService roleService)
        {
            _roleMenuService = roleMenuService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Roles = await _roleService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleMenuVM roleMenu)
        {
            if (await _roleMenuService.CreateAsync(roleMenu))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorContent = "Error creating Menu";
                return RedirectToAction("Error", new { message = errorContent });
            }
        }

        public async Task<JsonResult> GetMenusForRole(string roleId)
        {
            var menus = await _roleMenuService.GetMenusForRoleAsync(roleId);
            return Json(menus.Select(x => x.MenuId).ToList());
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { Message = message });
        }
    }
}
