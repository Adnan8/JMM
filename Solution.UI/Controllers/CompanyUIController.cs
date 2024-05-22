using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common;
using Solution.Common.ViewModel;
using Solution.UI.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solution.UI.Controllers
{
    [Authorize]
    public class CompanyUIController : Controller
	{
        private readonly ICompanyService _service;
        public CompanyUIController(ICompanyService service)
		{
            _service = service;
        }
		public IActionResult Index()
		{
			return View();
		}
        public async Task<IActionResult> GetAllCompanies(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var response = await _service.GetAll();
                if (response!=null)
                {

                    var filteredData = response.Where(c => string.IsNullOrEmpty(searchValue) || c.Compname.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();

                    var paginatedData = filteredData.Skip(start).Take(length).ToList();

                    var result = new
                    {
                        draw = draw,
                        recordsTotal = response.Count,
                        recordsFiltered = filteredData.Count,
                        data = paginatedData
                    };

                    return Json(result);
                }
                else
                {
                    // Handle the case where the API response is not successful
                    return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                // Return a response indicating no data is available
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
            }
        }

        public IActionResult Create()
        {
            var model = new CompanyVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyVM company, IFormFile logo)
        {

            var response = await _service.CreateAsync(company);

            if (response > 0)
            {
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating Company or Company User.";
                return RedirectToAction("Error", new { message = "" });
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            
            var response = await _service.EditAsync(id);

            if (response != null)
            {
                return View(response);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching company data" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyVM company)
        {

            var response = await _service.UpdateAsync(company);

            if (response)
            {
				TempData["success"] = "Company Updated successfully";
				return RedirectToAction("Index");
            }
            else
            {
				TempData["error"] = "Error updating company";
				return RedirectToAction("Index", new { message = "Error updating company" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            var response = await _service.DeleteAsync(id);

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
