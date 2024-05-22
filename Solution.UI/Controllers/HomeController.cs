using Microsoft.AspNetCore.Mvc;
using Solution.Business.Services.IServices;
using Solution.Common;
using Solution.UI.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Solution.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken != null)
                {
                    // Extract payload data
                    var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == CommonClaims.UserId)?.Value;
                    var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == CommonClaims.UserEmail)?.Value;
                    var companyId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == CommonClaims.CompId)?.Value;
                    var roles = jwtToken.Claims
                           .Where(claim => claim.Type == ClaimTypes.Role)
                           .Select(claim => claim.Value)
                           .FirstOrDefault();
                    // Assuming you have a ViewModel or you can use ViewBag/ViewData
                    HttpContext.Session.SetString("UserId", userId ?? "");
                    HttpContext.Session.SetString("UserEmail", userEmail ?? "");
                    HttpContext.Session.SetString("CompanyId", companyId ?? "");
                    HttpContext.Session.SetString("Roles", roles ?? "");

                    // Continue with your action logic...
                }
                else
                {
                    // Handle the case where the token couldn't be parsed
                    // This might involve redirecting to a login page, showing an error, etc.
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
