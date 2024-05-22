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
    [Authorize]

    public class DDLDtlsUIController : Controller
    {
        private readonly IDDLDtlsService _service;

        public DDLDtlsUIController(IDDLDtlsService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllDDLDtls(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var ddldtls = await _service.GetAll();
                if (ddldtls != null)
                {
                    var filteredData = ddldtls.Where(c => string.IsNullOrEmpty(searchValue) || c.Ddltxt.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    var paginatedData = filteredData.Skip(start).Take(length).ToList();

                    var result = new
                    {
                        draw = draw,
                        recordsTotal = ddldtls.Count,
                        recordsFiltered = filteredData.Count,
                        data = paginatedData
                    };

                    return Json(result);
                }
                else
                {
                    return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<DDLDtlsVM>() });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<DDLDtlsVM>() });
            }
        }

        public IActionResult Create()
        {
            var model = new DDLDtlsVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DDLDtlsVM ddldtls)
        {
            var result = await _service.CreateAsync(ddldtls);
            if (result)
            {
                TempData["success"] = "DDLDtls created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating DDLDtls";
                return RedirectToAction("Error", new { message = "Failed to create DDLDtls" });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ddldtls = await _service.EditAsync(id);
            if (ddldtls != null)
            {
                return View(ddldtls);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching DDLDtls data" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DDLDtlsVM ddldtls)
        {
            var result = await _service.UpdateAsync(ddldtls);
            if (result)
            {
                TempData["success"] = "DDLDtls updated successfully";
                return RedirectToAction("Details", new { id = ddldtls.Id });
            }
            else
            {
                TempData["error"] = "Error updating DDLDtls";
                return RedirectToAction("Index", new { message = "Error updating DDLDtls" });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var ddldtls = await _service.EditAsync(id);
            if (ddldtls != null)
            {
                return View(ddldtls);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching DDLDtls data" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "DDLDtls deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete DDLDtls" });
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
