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
    public class RolePermissionService : IRolePermissionService
    {

        private readonly ICommonService _common;
        private readonly IUnitofWork _unitofWork;
        private readonly IRoleService _roleService;
        HashIdToIntConverter obj = new HashIdToIntConverter();
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolePermissionService(ICommonService common, IUnitofWork unitofWork, IRoleService roleService, RoleManager<IdentityRole> roleManager)
        {
            _common = common;
            _unitofWork = unitofWork;
            _roleService = roleService;
            _roleManager = roleManager;
        }
        public async Task<bool> CreateAsync(RolePermissionVM rolepermissionDto)
        {
            var rolePermissionSample = _common.Map<RolePermission>(rolepermissionDto);
            var decryptedRoleId = rolePermissionSample.RoleId;
            if(decryptedRoleId != null)
            {
            await _unitofWork.RolePermissionRepository.DeleteByRoleId(decryptedRoleId);
            }
            var newPermissions = rolepermissionDto.RolePermissionId.Select(item => _common.Map<RolePermission>(new RolePermissionVM
            {
                PermissionId = item,
                RoleId = rolepermissionDto.RoleId, // Still encrypted, assuming your mapping handles it
            })).ToList();

            await _unitofWork.RolePermissionRepository.InsertRange(newPermissions);
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _unitofWork.RolePermissionRepository.GetById(id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.RolePermissionRepository.Update(model);
            return true;
        }

        public async Task<bool> UpdateAsync(RolePermissionVM rolepermissionDto)
        {
            var Id = obj.Convert(rolepermissionDto.Id, null);
            var model = await _unitofWork.RolePermissionRepository.GetById(Id);
            if (obj == null)
            {
                return false;
            }

            foreach (var item in rolepermissionDto.PermissionId)
            {
                var rolePermission = _common.Map<RolePermission>(model);
                await _unitofWork.RolePermissionRepository.Update(rolePermission);

            }
            return true;
        }
        public async Task<List<RolePermissionVM>> GetAll()
        {
            var allRolePermission = await _unitofWork.RolePermissionRepository.All.ToListAsync();
            var filteredRolePermission = allRolePermission.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filteredRolePermission == null || !filteredRolePermission.Any())
                return new List<RolePermissionVM>();
            var companies = filteredRolePermission.Select(d => _common.Map<RolePermissionVM>(d)).ToList();
            return companies;
        }
        public async Task<RolePermissionVM> EditAsync(int id)
        {
            var existingRolePermission = await _unitofWork.RolePermissionRepository.GetById(id);
            if (existingRolePermission == null)
                return new RolePermissionVM();

            var result = _common.Map<RolePermissionVM>(existingRolePermission);

            return result;
        }
        public async Task<List<RolePermissionVM>> GetPermissionsForRoleAsync(string roleId)
        {
            var permissions = await _unitofWork.RolePermissionRepository.All
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
            if (permissions == null || !permissions.Any())
                new List<RolePermissionVM>();
            var companies = permissions.Select(d => _common.Map<RolePermissionVM>(d)).ToList();
            return companies;
        }
        public async Task<List<RoleVM>> GetRolesForPermissionsAsync(int permissionId)
        {
            var roleIds = await _unitofWork.RolePermissionRepository.All
                                  .Where(rm => rm.PermissionId == permissionId)
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

    }
}
