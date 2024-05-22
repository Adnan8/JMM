using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Solution.Business.Enums;
using Solution.Business.Services;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.UI.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Solution.UI.Controllers
{
    public class UserUIController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _service;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserUIController(IUserService service, IRoleService roleService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
		{
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _service = service;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> GetAllUsers(int draw, int start, int length, string searchValue = "")
		{
			try
			{
                var response = await _service.GetAllAsync();
				if (response != null)
				{

					var filteredData = response.Where(c => string.IsNullOrEmpty(searchValue) || c.UserName.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();

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
					return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
				}
			}
			catch (Exception ex)
			{
				return Json(new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = new List<CompanyVM>() });
			}
		}

        public async Task<IActionResult> Create()
        {
            var response = await _roleService.GetAll();

            if (response != null)
            {
                var rolesWithIds = response.Select(x => new { Name = x.Name, Id = x.Id }).ToList();

                ViewBag.Roles = rolesWithIds.Where(x=>x.Name!="SuperAdmin");

            }
            
            var model = new UserVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM model)
        {
            if (model.RoleId == "User")
            {
                model.UserType=UserTypes.User; 
            }
            if (model.RoleId == "Admin")
            {
                model.UserType = UserTypes.Admin;
            }
            var userClamis = _httpContextAccessor.HttpContext?.User;
            var userId = userClamis.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            model.CompId = user.CompId.ToString();
            var response = await _service.RegisterUserAsync(model);

            if (response != null)
            {
                TempData["success"] = "User created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "error creating User";
                return RedirectToAction("Index", new { message = "Error creating User. Details: "  });
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

            ViewData["Title"] = "Error";

            return View(errorViewModel);
        }

    }
}

