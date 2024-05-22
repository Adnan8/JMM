using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Solution.Business.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitofWork _unitofWork;
        private readonly ICommonService _commonService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public UserContextService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IUnitofWork unitofWork, ICommonService commonService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitofWork = unitofWork;
            _commonService = commonService;
        }

        public string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //public string GetCompanyId()
        //{
        //    var context = this._httpContextAccessor.HttpContext;
        //    var UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    string Compid="";
        //    if (UserId != null)
        //    {
        //        var Id = obj.Convert(UserId, null);
        //        var companie = _unitofWork.CompanyRepository.All.FirstOrDefault(x=> x.Id==Id);
        //        if (companie != null)
        //        {
        //            var result = _commonService.Map<CompanyVM>(companie);
        //            Compid = result.Id;
        //        }
        //        return Compid;
        //    }
        //    return "";
        //    //var compIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == CommonClaims.CompId);
        //    //return compIdClaim?.Value;
        //}
        //public string GetCompanyId()
        //{
        //    var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(CommonClaims.CompId);
        //    return companyIdClaim;
        //}
        public string GetCompanyId()
        {
            var rawToken = GetRawJwtToken();
            if (string.IsNullOrEmpty(rawToken)) return null;

            var claims = DecodeJwtToken(rawToken);
            if (claims == null) return null;

            claims.TryGetValue(CommonClaims.CompId, out var companyId);
            return companyId;
        }

        public async Task<(string RoleId, string RoleName)> GetRoleInfoAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return (null, null);

            var roleNames = await _userManager.GetRolesAsync(user);
            var roleName = roleNames.FirstOrDefault(); // Assuming a single role per user

            if (roleName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    return (role.Id, role.Name);
                }
            }

            return (null, null);
        }

        private string GetRawJwtToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }

            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        private IDictionary<string, string> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null) return null;

            IDictionary<string, string> tokenClaims = jwtToken.Claims
                .ToDictionary(claim => claim.Type, claim => claim.Value);

            return tokenClaims;
        }

    }
}
