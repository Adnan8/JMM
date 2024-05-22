using Microsoft.AspNetCore.Mvc;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.UI.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Solution.UI.Controllers
{
    public class RolesUIController : Controller
    {
        private readonly IRoleService _roleService;
        public RolesUIController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllRoles(int draw, int start, int length, string searchValue = "")
        {
            try
            {

                var response = await _roleService.GetAll();

                if (response!=null)
                {
                    

                    var filteredData = response.Where(r => string.IsNullOrEmpty(searchValue) || r.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();

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
                    // Instead of returning a generic error, return a JSON structure that matches the DataTables expected format
                    // but indicates no records are available.
                    return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<RoleVM>() });
                }
            }
            catch (Exception ex)
            {
                // Log the exception here with your preferred logging method

                // Return a response that keeps the DataTables structure intact for error handling
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<RoleVM>() });
            }
        }

        public IActionResult Create()
        {
            var model = new RoleVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVM role)
        {
            
               var response = await _roleService.CreateAsync(role); 
            

            if (response)
            {
                TempData["success"] = "Role created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { message = "Error creating role. Details: " + "" });
            }
        }
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _roleService.EditAsync(id);

            if (response != null)
            {
                return View(response);
            }
            else
            {
                TempData["error"] = "error fetching role data";
                return RedirectToAction("Index", new { message = "Error fetching role data" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleVM role)
        {
            var response = await _roleService.UpdateAsync(role);

            if (response)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error updating role" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            
            var response = await _roleService.DeleteAsync(id);  

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

            // Use ViewData or ViewBag if you need to pass additional data to the view
            ViewData["Title"] = "Error";

            return View(errorViewModel);
        }

    }
}
