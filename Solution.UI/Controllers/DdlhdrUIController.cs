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
    public class DdlhdrUIController : Controller
    {
        private readonly IDdlhdrService _ddlhdrService;

        public DdlhdrUIController(IDdlhdrService ddlhdrService)
        {
            _ddlhdrService = ddlhdrService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllDdlhdr(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var ddlhdrs = await _ddlhdrService.GetAll();
                var filteredData = ddlhdrs.Where(c => string.IsNullOrEmpty(searchValue) || c.Ddlname.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                var paginatedData = filteredData.Skip(start).Take(length).ToList();

                var result = new
                {
                    draw = draw,
                    recordsTotal = ddlhdrs.Count,
                    recordsFiltered = filteredData.Count,
                    data = paginatedData
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<DdlhdrVM>() });
            }
        }

        public IActionResult Create()
        {
            var model = new DdlhdrVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DdlhdrVM ddlhdr)
        {
            if (await _ddlhdrService.CreateAsync(ddlhdr))
            {
                TempData["success"] = "Ddlhdr created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating Ddlhdr";
                return RedirectToAction("Error", new { message = "Failed to create Ddlhdr" });
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var ddlhdr = await _ddlhdrService.EditAsync(id);
            if (ddlhdr != null)
            {
                return View(ddlhdr);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching Ddlhdr data" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DdlhdrVM ddlhdr)
        {
            if (await _ddlhdrService.UpdateAsync(ddlhdr))
            {
                TempData["success"] = "Ddlhdr updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error updating Ddlhdr";
                return RedirectToAction("Index", new { message = "Error updating Ddlhdr" });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var details = await _ddlhdrService.GetDdldtlsByDdlhdrIdAsync(id);
            if (details != null)
            {
                // Assuming you have a view that can take a list of details directly.
                return View(details);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching details for Ddlhdr" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _ddlhdrService.DeleteAsync(id))
            {
                return Json(new { success = true, message = "Ddlhdr deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete Ddlhdr" });
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
