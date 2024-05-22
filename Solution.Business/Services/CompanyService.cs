using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using Solution.Repository.Repo;
using Solution.Repository.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.Business.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommonService _common;
        private readonly IUserService _userService;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public CompanyService(ICommonService common, IUnitofWork unitofWork, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _common = common;
            _unitofWork = unitofWork;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<int> CreateAsync(CompanyVM companyDto)
        {
            try
            {

                var model = _common.Map<Company>(companyDto);
                await _unitofWork.CompanyRepository.Insert(model);

                var companyNameForEmail = companyDto.Compname.Replace(" ", "") + "@gmail.com";
                var registerViewModel = new UserVM
                {
                    Email = companyNameForEmail,
                    Password = "P@kistan1",
                    ConfirmPassword = "P@kistan1",
                    RoleId = "SuperAdmin",
                    DisplayName = companyDto.Compname,
                    CompId = model.Id.ToString(),
                    IsCompany = true,
                    UserType = Enums.UserTypes.Company
                };

                var registerResult = await _userService.RegisterUserAsync(registerViewModel);
                if (registerResult.Result.Succeeded)
                {
                    return model.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var company = await _unitofWork.CompanyRepository.GetById(Id);
            if (company == null)
            {
                return false; 
            }
            company.IsDeleted = true;
            await _unitofWork.CompanyRepository.Update(company);

            string userEmail = company.Compname + "@gmail.com";

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }



        public async Task<bool> UpdateAsync(CompanyVM companyDto)
        {
            var Id = obj.Convert(companyDto.Id, null);
            var model = await _unitofWork.CompanyRepository.GetById(Id);
                    if (obj == null)
                    {
                        return false;
                    }
                    var company = _common.Map<Company>(model);
                    company.Compname = companyDto.Compname;
                    company.Abn = companyDto.Abn;
                    company.Addr1 = companyDto.Addr1;
                    company.Addr2 = companyDto.Addr2;
                    company.City = companyDto.City;
                    company.IsDeleted = companyDto.IsDeleted;
                    company.State = companyDto.State;
                    company.Country = companyDto.Country;
                    company.DefaultPermissions = companyDto.DefaultPermissions;
                    await _unitofWork.CompanyRepository.Update(company);
                    return true;
        }
        public async Task<List<CompanyVM>> GetAll()
        {
                var allCompany = await _unitofWork.CompanyRepository.All.ToListAsync();
            var filteredCompany=allCompany.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filteredCompany == null || !filteredCompany.Any())
                   return new List<CompanyVM>();
                var companies = filteredCompany.Select(d => _common.Map<CompanyVM>(d)).ToList();
                return companies;
        }
        public async Task<CompanyVM> EditAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var existingCompany = await _unitofWork.CompanyRepository.GetById(Id);
                if (existingCompany == null)
                    return new CompanyVM();

                var result = _common.Map<CompanyVM>(existingCompany);

                return result;
        }

       
    }
}
