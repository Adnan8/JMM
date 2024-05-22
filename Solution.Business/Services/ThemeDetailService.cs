using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.Business.Services
{
    public class ThemeDetailService : IThemeDetailService
    {
        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThemeDetailService(ICommonService common, IUnitofWork unitofWork, IHttpContextAccessor httpContextAccessor, IUserContextService userContextService, UserManager<ApplicationUser> userManager)
        {
            _common = common;
            _unitofWork = unitofWork;
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
            _userManager = userManager;
        }

        public async Task<bool> CreateAsync(ThemeDetailVM themeDetailDto)
        {
            themeDetailDto.UserId = _userContextService.GetUserId();
            var CompId = _userContextService.GetCompanyId();
            themeDetailDto.CompId = Convert.ToInt32(CompId);
            var Id = obj.Convert(themeDetailDto.Id, null);
            ThemeDetail model;
            if (string.IsNullOrEmpty(themeDetailDto.Id))
            {
                model = _unitofWork.ThemeDetailRepository.All
                            .FirstOrDefault(x => x.UserId == themeDetailDto.UserId && (x.IsDeleted == false || x.IsDeleted == null));
            }
            else
            {
                model = _unitofWork.ThemeDetailRepository.All
                            .FirstOrDefault(x => x.Id == Id && x.UserId == themeDetailDto.UserId && (x.IsDeleted == false || x.IsDeleted == null));
            }

            if (model == null)
            {
                // If no existing ThemeDetail, create a new one.
                model = new ThemeDetail
                {
                    // Assuming CompId is required for creating a new ThemeDetail.
                    CompId = themeDetailDto.CompId.ToString(), // Or some default value if it's not set in themeDetailDto.
                    UserId = themeDetailDto.UserId,
                    Primarybg = themeDetailDto.Primarybg,
                    Primaryfg = themeDetailDto.Primaryfg,
                    Secondarybg = themeDetailDto.Secondarybg,
                    Secondaryfg = themeDetailDto.Secondaryfg,
                    Tertiarybg = themeDetailDto.Tertiarybg,
                    Tertiaryfg = themeDetailDto.Tertiaryfg
                };
                await _unitofWork.ThemeDetailRepository.Insert(model);
            }
            else
            {
                // If an existing ThemeDetail was found, update its properties.
                model.Primarybg = themeDetailDto.Primarybg;
                model.Primaryfg = themeDetailDto.Primaryfg;
                model.Secondarybg = themeDetailDto.Secondarybg;
                model.Secondaryfg = themeDetailDto.Secondaryfg;
                model.Tertiarybg = themeDetailDto.Tertiarybg;
                model.Tertiaryfg = themeDetailDto.Tertiaryfg;

                await _unitofWork.ThemeDetailRepository.Update(model);
            }

            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _unitofWork.ThemeDetailRepository.GetById(id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.ThemeDetailRepository.Update(model);
            return true;  
        }
        public async Task<ThemeDetailVM> GetByUserId(string id, string CompanyId)
        {
            try
            {
                var model = new ThemeDetail();

                if (CompanyId == "0")
                {
                    model = _unitofWork.ThemeDetailRepository.All.Where(x => x.UserId == id && (x.IsDeleted == false || x.IsDeleted == null)).FirstOrDefault();
                }
                else
                {

                    var Company = _unitofWork.CompanyRepository.All.Where(x => x.Id == Convert.ToInt32(CompanyId)).FirstOrDefault();
                    if (Company != null && Company.DefaultPermissions != null && Company.DefaultPermissions == true)
                    {
                        model = _unitofWork.ThemeDetailRepository.All.Where(x => x.UserId == id && (x.IsDeleted == false || x.IsDeleted == null)).FirstOrDefault();
                    }
                    else
                    {
                        model = new ThemeDetail();
                    }
                }
                if (model != null)
                {
                    var result = _common.Map<ThemeDetailVM>(model);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return new ThemeDetailVM();
        }
        public async Task<bool> UpdateAsync(ThemeDetailVM themeDetailDto)
        {
            var Id = obj.Convert(themeDetailDto.Id, null);
            var model = await _unitofWork.ThemeDetailRepository.GetById(Id);
                    if (obj == null)
                    {
                        return false;
                    }
                    var themeDetail = _common.Map<ThemeDetail>(model);
                    themeDetail.UserId = themeDetailDto.UserId;
                    themeDetail.Primarybg = themeDetailDto.Primarybg;
                    themeDetail.Primaryfg = themeDetailDto.Primaryfg;
                    themeDetail.Secondarybg = themeDetailDto.Secondarybg;
                    themeDetail.Secondaryfg = themeDetailDto.Secondaryfg;
                    themeDetail.Tertiarybg = themeDetailDto.Tertiarybg;
                    themeDetail.Tertiaryfg = themeDetailDto.Tertiaryfg;
                    await _unitofWork.ThemeDetailRepository.Update(themeDetail);
                    return true;
        }
        public async Task<List<ThemeDetailVM>> GetAll()
        {
                var allThemedetail = await _unitofWork.ThemeDetailRepository.All.ToListAsync();
            var filteredthemeDetail = new List<ThemeDetail>();
            var CompId= _userContextService.GetCompanyId();
            var UserId = _userContextService.GetUserId();
            var Email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (CompId == "0")
            {
                 filteredthemeDetail = allThemedetail.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
                var Username = await _userManager.FindByEmailAsync(Email);
            }
            else
            {
                filteredthemeDetail = allThemedetail.Where(x =>x.CompId==CompId && x.UserId == UserId && (x.IsDeleted == false || x.IsDeleted == null)).ToList();
            }
                if (filteredthemeDetail == null || !filteredthemeDetail.Any())
                    return new List<ThemeDetailVM>();
                var themeDetails = filteredthemeDetail.Select(d => _common.Map<ThemeDetailVM>(d)).ToList();
                return themeDetails;
        }
        public async Task<ThemeDetailVM> EditAsync(int id)
        {
                var existingthemeDetail = await _unitofWork.ThemeDetailRepository.GetById(id);
                if (existingthemeDetail == null)
                    return new ThemeDetailVM();

                var result = _common.Map<ThemeDetailVM>(existingthemeDetail);

                return result;
        }
    }
}
