using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Solution.Business.Services.IServices;
using Solution.Common;
using Solution.Common.ViewModel;
using Solution.UI.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.UI.Controllers
{
    [Authorize]
    public class MenuUIController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;
        private readonly Hashids _hashids;

        public MenuUIController(IMenuService menuService, IRoleService roleService)
        {
            _roleService = roleService;
            _menuService = menuService;
            _hashids = new Hashids(ConstantUnique.HashidsName, ConstantUnique.HashidsLength);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllMenu(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var response = await _menuService.GetAll();
                if (response != null)
                {
                    var filteredData = response.Where(c => string.IsNullOrEmpty(searchValue) || c.Title.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    var paginatedData = filteredData.Skip(start).Take(length).ToList();

                    var result = new
                    {
                        draw = draw,
                        recordsTotal = response.Count(),
                        recordsFiltered = filteredData.Count,
                        data = paginatedData
                    };

                    return Json(result);
                }
                else
                {
                    return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<MenuVM>() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<MenuVM>() });
            }
        }

        public async Task<IActionResult> Create()
        {
            var model = new MenuVM();

            var roles = await _roleService.GetAll();
            var menus = await _menuService.GetAll();

            model.Roles = roles;
            model.ParentMenus = menus.Select(m => new SelectListItem
            {
                Value = _hashids.Decode(m.Id).FirstOrDefault().ToString(),
                Text = m.Title
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuVM menu)
        {
            var response = await _menuService.CreateAsync(menu);

            if (response)
            {
                TempData["success"] = "Menu created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating menu";
                return RedirectToAction("Index", new { message = "Error creating menu. Details: " });
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var menu = await _menuService.EditAsync(id);
            var roles = await _roleService.GetAll();
            var allMenus = await _menuService.GetAll();

            if (menu == null || roles == null || allMenus == null)
            {
                return RedirectToAction("Index", new { message = "Error processing the data" });
            }

            menu.ParentMenus = allMenus.Where(m => m.Id != id).Select(m => new SelectListItem
            {
                Value = _hashids.Decode(m.Id.ToString()).FirstOrDefault().ToString(),
                Text = m.Title
            }).ToList();

            foreach (var role in roles)
            {
                role.IsSelected = menu.Roles.Any(r => r.Id == role.Id);
            }

            menu.Roles = roles;

            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuVM menu)
        {
            var response = await _menuService.UpdateAsync(menu);

            if (response)
            {
                TempData["success"] = "Menu updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error updating menu";
                return RedirectToAction("Index", new { message = "Error updating menu" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _menuService.DeleteAsync(id);

            if (response)
            {
                return Json(new { success = true, message = "Delete successful" });
            }
            else
            {
                return Json(new { success = false, message = "Delete failed" });
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            };

            ViewData["Title"] = "Error";

            return View(errorViewModel);
        }
    }
}
