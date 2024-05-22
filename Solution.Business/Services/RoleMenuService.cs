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
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Solution.Business.Services
{
    public class RoleMenuService : IRoleMenuService
    {
        
        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        private readonly IRoleService _roleService;
        private readonly RoleManager<IdentityRole> _roleManager;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public RoleMenuService(ICommonService common, IUnitofWork unitofWork, IRoleService roleService, RoleManager<IdentityRole> roleManager)
        {
            _common = common;
            _unitofWork = unitofWork;
            _roleService = roleService;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(RoleMenuVM roleMenuDto)
        {
            var roleMenuSample = _common.Map<RoleMenu>(roleMenuDto);
            var decryptedRoleId = roleMenuSample.RoleId;
            if (decryptedRoleId != null)
            {
                await _unitofWork.RoleMenuRepository.DeleteByRoleId(decryptedRoleId);
            }
            var newPermissions = roleMenuDto.RoleMenuId.Select(item => _common.Map<RoleMenu>(new RoleMenuVM
            {
                MenuId = item,
                RoleId = roleMenuDto.RoleId,
            })).ToList();
            await _unitofWork.RoleMenuRepository.InsertRange(newPermissions);
            return true;
        }
        public async Task<List<RoleMenuVM>> GetMenusForRoleAsync(string roleId)
        {
            try
            {
                var Menus = await _unitofWork.RoleMenuRepository.All
               .Where(rp => rp.RoleId == roleId)
               .ToListAsync();
                if (Menus == null || !Menus.Any())
                    return null;
                var menu = Menus.Select(d => _common.Map<RoleMenuVM>(d)).ToList();
                return menu;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public async Task<List<RoleVM>> GetRolesForMenuAsync(int menuId)
        {
            var roleIds = await _unitofWork.RoleMenuRepository.All
                                  .Where(rm => rm.MenuId == menuId)
                                  .Select(rm => rm.RoleId)
                                  .Distinct()
                                  .ToListAsync();

            var roles = new List<RoleVM>();
            foreach (var roleId in roleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                {
                    roles.Add(new RoleVM
                    {
                        Id = role.Id,
                        Name = role.Name,
                    });
                }
            }

            return roles;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _unitofWork.RoleMenuRepository.GetById(id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.RoleMenuRepository.Update(model);
            return true;  
        }

        public async Task<bool> UpdateAsync(RoleMenuVM roleMenuDto)
        {
            //var Id = obj.Convert(roleMenuDto.Id, null);
            var model =  _unitofWork.RoleMenuRepository.All.Where(x => x.RoleId == roleMenuDto.RoleId).FirstOrDefault();
                    if (obj == null)
                    {
                        return false;
                    }

            foreach (var item in roleMenuDto.MenuId)
            {
                    var roleMenu = _common.Map<RoleMenu>(model);
                    await _unitofWork.RoleMenuRepository.Update(roleMenu);

            }
                    return true;
        }
        public async Task<List<RoleMenuVM>> GetAll()
        {
                var allCompany = await _unitofWork.RoleMenuRepository.All.ToListAsync();
            var filteredCompany=allCompany.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filteredCompany == null || !filteredCompany.Any())
                return null;
                var companies = filteredCompany.Select(d => _common.Map<RoleMenuVM>(d)).ToList();
                return companies;
        }
        public async Task<RoleMenuVM> EditAsync(int id)
        {
                var existingCompany = await _unitofWork.RoleMenuRepository.GetById(id);
            if (existingCompany == null)
                return null;

                var result = _common.Map<RoleMenuVM>(existingCompany);

                return result;
        }

       
    }
}
