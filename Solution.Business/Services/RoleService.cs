using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solution.Business.Mapper;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommonService _common;

        public RoleService(RoleManager<IdentityRole> roleManager, ICommonService common)
        {
            _roleManager = roleManager;
            _common = common;
        }

        public async Task<bool> CreateAsync(RoleVM roleDto)
        {
            var role = new IdentityRole(roleDto.Name);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return false;
                }
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdateAsync(RoleVM roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleDto.Id);
            if (role == null)
            {
                return false;
            }

            role.Name = roleDto.Name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }


        public async Task<List<RoleVM>> GetAll()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();

                var roleVms = roles.Select(role => new RoleVM
                {
                    Id = role.Id,
                    Name = role.Name,
                }).ToList();

                return roleVms;
            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public async Task<RoleVM> EditAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return null;
            }

            var roleVm = new RoleVM
            {
                Id = role.Id,
                Name = role.Name,
            };

            return roleVm;
        }

    }
}
