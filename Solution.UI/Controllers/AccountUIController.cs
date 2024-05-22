using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solution.Common.ViewModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
using System.Text.Json;
using Solution.Business.Services.IServices;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authorization;
using Solution.Business.Enums;
using Solution.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Solution.DAL.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.UI.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class AccountUIController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserService _service;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountUIController(IHttpContextAccessor httpContentAccessor, IWebHostEnvironment hostingEnvironment, IUserService service, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContentAccessor;
            _hostingEnvironment = hostingEnvironment;
            _service = service;
        }
        public IActionResult Index(string tenantId)
        {
            ViewBag.TenantId = tenantId;
            return View(new UserVM());
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserVM model)
        {
            var response = await _service.LoginUserAsync(model);
            if (response != null && response.LoginResult == LoginResult.Success)
            {
                if (response.RoleData != null)
                {
                    var RoleData = JsonConvert.SerializeObject(response.RoleData); //RoleData==roleid,rolename,rolemenu,rolepermission
                    HttpContext.Session.SetString("RoleData", RoleData);
                }
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null && user.CompId > 0)
                {
                    var company = _context.Companies
                        .Where(c => c.Id == user.CompId)
                        .Select(c => new { c.Compname, c.Id })
                        .FirstOrDefault();
                    HttpContext.Session.SetString("TenantId", company.Compname.ToString());
                }
                else
                {
                    HttpContext.Session.SetString("TenantId", "Enterprise");
                }

                // Redirect including the tenantId
                return RedirectToAction("Index", "Home", new { tenantId = HttpContext.Session.GetString("TenantId") });
            }
            switch (response?.LoginResult)
            {
                case LoginResult.WrongPassword:
                    ModelState.AddModelError(string.Empty, "The password is incorrect.");
                    break;
                case LoginResult.UserNotFound:
                    ModelState.AddModelError(string.Empty, "No user found with that email address.");
                    break;
                case LoginResult.LockedOut:
                    ModelState.AddModelError(string.Empty, "This account has been locked. Please try again later.");
                    break;
                case LoginResult.NotAllowed:
                    ModelState.AddModelError(string.Empty, "Login not allowed.");
                    break;
                case LoginResult.RequiresTwoFactor:
                    ModelState.AddModelError(string.Empty, "Two-factor authentication is required.");
                    break;
                default:
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    break;
            }
            return View("Index", model);

        }
        public IActionResult Register()
        {
            return View(new UserVM());
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserVM model)
        {
            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
            if (!Regex.IsMatch(model.Password, passwordPattern))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and include at least one letter, one number, and one special character.");
                return View(model);
            }
            model.UserType =UserTypes.Enterprise;
            var response = await _service.RegisterUserAsync(model);

            if (response.Result.Succeeded)
            {
                TempData["success"] = "Registration successful. Please log in.";
                return RedirectToAction("Index", "AccountUI");
            }
            foreach (var error in response.Result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
        public async Task<IActionResult> UserProfile()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var isAuthenticated = user.Identity.IsAuthenticated;
            var Id = user.FindFirst("UserId")?.Value;

            if (!isAuthenticated || string.IsNullOrEmpty(Id))
            {
                return Redirect("/login");
            }

            UserProfileVM data = null;
            data.RoleId ??= "Enterprise";
            data.CompanyName ??= "No Company";

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileVM model, IFormFile? profileImage)
        {
            if (profileImage != null)
            {
                model.ProfileImage = profileImage;
            }

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.DisplayName ?? string.Empty), nameof(model.DisplayName));
            content.Add(new StringContent(model.Email ?? string.Empty), nameof(model.Email));
            content.Add(new StringContent(model.Username ?? string.Empty), nameof(model.Username));
            content.Add(new StringContent(model.CompanyName ?? string.Empty), nameof(model.CompanyName));
            content.Add(new StringContent(model.RoleId ?? string.Empty), nameof(model.RoleId));
            if (model.ProfileImage != null)
            {
                var fileContent = new StreamContent(model.ProfileImage.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ProfileImage.ContentType);
                content.Add(fileContent, nameof(model.ProfileImage), model.ProfileImage.FileName);
            }

            TempData["error"] = "Error Updating Profile";
            return View(model);


        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordVM());
        }
        public IActionResult Registration()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("RoleData");
            HttpContext.Session.Remove("TenantId");

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            var tenantId = HttpContext.Session.GetString("TenantId");
            return RedirectToAction("Index", "AccountUI", new { tenantId = tenantId });
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
