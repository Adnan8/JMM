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
    public class MenuService: IMenuService
    {
        private readonly ICommonService _common;
        private readonly IRoleMenuService _roleMenuService;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();
        

        public MenuService(ICommonService common, IUnitofWork unitofWork, IRoleMenuService roleMenuService)
        {
            _common=common;
            _unitofWork = unitofWork;
            _roleMenuService = roleMenuService;
        }

        public async Task<bool> CreateAsync(MenuVM menuDto)
        {
            try
            {
                if (menuDto.ParentId == null)
                {
                    menuDto.MenuLevel = "1";
                }
                else
                {
                    //var Id = obj.Convert(menuDto.ParentId, null);
                    var data = _unitofWork.MenuRepository.All.Where(x => x.Id == menuDto.ParentId).FirstOrDefault();
                    var MenuLevel = Convert.ToInt32(data.MenuLevel) + 1;
                    menuDto.MenuLevel = MenuLevel.ToString();
                }
                var menu = _common.Map<Menu>(menuDto);
                await _unitofWork.MenuRepository.Insert(menu);
                if (menuDto.SelectedRoles != null && menu.Id > 0) 
                {
                    foreach (var roleId in menuDto.SelectedRoles)
                    {
                        var roleMenu = new RoleMenu
                        {
                            MenuId = menu.Id,
                            RoleId = roleId 
                        };
                        await _unitofWork.RoleMenuRepository.Insert(roleMenu);
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var model = await _unitofWork.MenuRepository.GetById(Id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.MenuRepository.Update(model);
            return true;  
        }

        public async Task<bool> UpdateAsync(MenuVM menuDto)
        {
            var Id = obj.Convert(menuDto.Id, null);
            //var ParentId = obj.Convert(menuDto.ParentId, null);
            var model = await _unitofWork.MenuRepository.GetById(Id);
            if (model == null)
            {
                return false;
            }
            model.Title = menuDto.Title;
            model.Descr = menuDto.Descr;
            if(menuDto.ParentId == null)
            {
                model.MenuLevel = "1";
            }
            else 
            {
                var data = _unitofWork.MenuRepository.All.Where(x => x.Id == menuDto.ParentId).FirstOrDefault();
                var MenuLevel = Convert.ToInt32(data.MenuLevel) + 1;
                model.MenuLevel = MenuLevel.ToString();
            }
            model.ParentId = menuDto.ParentId;
            model.Icon = menuDto.Icon;
            model.Url = menuDto.Url;
            model.MenuOrder = menuDto.MenuOrder;
            model.Controller = menuDto.Controller;
            model.Page = menuDto.Page;
            model.IsDefault = menuDto.IsDefault;

            await _unitofWork.MenuRepository.Update(model);
            var existingRoleMenus =  _unitofWork.RoleMenuRepository.Find(rm => rm.MenuId == model.Id && (rm.IsDeleted == false || rm.IsDeleted == null));
            _unitofWork.RoleMenuRepository.RemoveRange(existingRoleMenus.Where(rm => !menuDto.SelectedRoles.Contains(rm.RoleId)));

            foreach (var roleId in menuDto.SelectedRoles)
            {
                if (!existingRoleMenus.Any(rm => rm.RoleId == roleId))
                {
                    var roleMenu = new RoleMenu
                    {
                        MenuId = model.Id,
                        RoleId = roleId
                    };
                    await _unitofWork.RoleMenuRepository.Insert(roleMenu);
                }
            }
            return true;
        }
        public async Task<List<MenuVM>> GetAll()
        {
                var allMenu = await _unitofWork.MenuRepository.All.ToListAsync();
            var filteredMenu=allMenu.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filteredMenu == null || !filteredMenu.Any())
                return null;
                var companies = filteredMenu.Select(d => _common.Map<MenuVM>(d)).ToList();
                return companies;
        }
        public async Task<MenuVM> EditAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var existingMenu = await _unitofWork.MenuRepository.GetById(Id);
                if (existingMenu == null)
                    return new MenuVM();
                var result = _common.Map<MenuVM>(existingMenu);
            var roles = await _roleMenuService.GetRolesForMenuAsync(Id);
            result.Roles =  roles;
            return result;
        }

       
    }
}
