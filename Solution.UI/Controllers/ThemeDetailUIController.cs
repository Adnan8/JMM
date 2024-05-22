using Microsoft.AspNetCore.Mvc;
using Solution.Common.ViewModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Solution.DAL.Models;
using Solution.UI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.UI.Controllers
{
    public class ThemeDetailUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ThemeDetailUIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllThemeDetail(int draw, int start, int length, string searchValue = "")
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync("ThemeDetail/All");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var themeDetails = JsonSerializer.Deserialize<List<ThemeDetailVM>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var filteredData = themeDetails.Where(c => string.IsNullOrEmpty(searchValue) || c.UserId.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();

                    var paginatedData = filteredData.Skip(start).Take(length).ToList();

                    var result = new
                    {
                        draw = draw,
                        recordsTotal = themeDetails.Count,
                        recordsFiltered = filteredData.Count,
                        data = paginatedData
                    };

                    return Json(result);
                }
                else
                {
                    return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
            }
        }
        public IActionResult Create()
        {
            var model = new ThemeDetailVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThemeDetailVM themeDetail)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            themeDetail.Id = themeDetail.Id!=null? themeDetail.Id : string.Empty;

            var content = new StringContent(JsonSerializer.Serialize(themeDetail), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync("ThemeDetail/Insert", content);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Error calling API" });
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Successfull ! please login again to see changes";
                return RedirectToAction("Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = "error creating Theme Detail";
                return RedirectToAction("Error", new { message = "Error creating Theme Details: " + errorContent });
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"ThemeDetail/Edit/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var themeDetail = JsonSerializer.Deserialize<ThemeDetailVM>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(themeDetail);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching company data" });
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"ThemeDetail/Edit/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var themeDetail = JsonSerializer.Deserialize<ThemeDetailVM>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(themeDetail);
            }
            else
            {
                return RedirectToAction("Index", new { message = "Error fetching themeDetail data" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ThemeDetailVM themeDetail)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var content = new StringContent(JsonSerializer.Serialize(themeDetail), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("ThemeDetail/Update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "ThemeDetail Updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error updating ThemeDetail";
                return RedirectToAction("Index", new { message = "Error updating ThemeDetail" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"ThemeDetail/Delete/{id}");

            if (response.IsSuccessStatusCode)
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
