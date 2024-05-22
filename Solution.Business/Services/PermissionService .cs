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
    public class PermissionService : IPermissionService
    {
        private readonly ICommonService _common;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IUnitofWork _unitofWork;
        HashIdToIntConverter obj = new HashIdToIntConverter();

        public PermissionService(ICommonService common, IUnitofWork unitofWork, IRolePermissionService rolePermissionService)
        {
            _common = common;
            _unitofWork = unitofWork;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<bool> CreateAsync(PermissionVM permissionDto)
        {
                var permission = _common.Map<Permission>(permissionDto);
                await _unitofWork.PermissionRepository.Insert(permission);
            if (permissionDto.SelectedRoles != null && permission.Id > 0)
            {
                foreach (var roleId in permissionDto.SelectedRoles)
                {
                    var roleMenu = new RolePermission
                    {
                        PermissionId = permission.Id,
                        RoleId = roleId
                    };
                    await _unitofWork.RolePermissionRepository.Insert(roleMenu);
                }
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var Id = obj.Convert(id, null);
            var model = await _unitofWork.PermissionRepository.GetById(Id);
            if (model == null)
            {
                return false;
            }
            model.IsDeleted = true;
            await _unitofWork.PermissionRepository.Update(model);
            return true;  
        }

        public async Task<bool> UpdateAsync(PermissionVM permissionDto)
        {
            try
            {
                var id = obj.Convert(permissionDto.Id, null);
                var existingPermission = await _unitofWork.PermissionRepository.GetById(id);

                if (existingPermission == null)
                {
                    // Log that the permission was not found
                    Console.WriteLine($"Permission with ID {id} not found.");
                    return false;
                }

                var permission = _common.Map<Permission>(existingPermission);
                permission.PermissionName = permissionDto.PermissionName;
                permission.Action = permissionDto.Action;
                permission.Controller = permissionDto.Controller;
                permission.OperationType = permissionDto.OperationType;
                permission.IsDefault = permissionDto.IsDefault;

                await _unitofWork.PermissionRepository.Update(permission);

                var existingRolePermissions = _unitofWork.RolePermissionRepository
                    .Find(rp => rp.PermissionId == existingPermission.Id && (rp.IsDeleted == false || rp.IsDeleted == null))
                    .ToList();

                var rolesToRemove = existingRolePermissions.Where(rp => !permissionDto.SelectedRoles.Contains(rp.RoleId)).ToList();
                _unitofWork.RolePermissionRepository.RemoveRange(rolesToRemove);

                foreach (var roleId in permissionDto.SelectedRoles)
                {
                    if (!existingRolePermissions.Any(rp => rp.RoleId == roleId))
                    {
                        var rolePermission = new RolePermission
                        {
                            PermissionId = existingPermission.Id,
                            RoleId = roleId
                        };
                        await _unitofWork.RolePermissionRepository.Insert(rolePermission);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public async Task<List<PermissionVM>> GetAll()
        {
                var allpermission = await _unitofWork.PermissionRepository.All.ToListAsync();
            var filteredpermission = allpermission.Where(x => x.IsDeleted == false || x.IsDeleted == null).ToList();
            if (filteredpermission == null || !filteredpermission.Any())
                    throw new Exception("No Ddlhdrs found");
                var permission = filteredpermission.Select(d => _common.Map<PermissionVM>(d)).ToList();
                return permission;
        }
        public async Task<PermissionVM> EditAsync(string id)
        {
            var Id = obj.Convert(id, null);

            var existingPermission = await _unitofWork.PermissionRepository.GetById(Id);
                if (existingPermission == null)
                    throw new Exception("Company not found");

                var result = _common.Map<PermissionVM>(existingPermission);
            var roles = await _rolePermissionService.GetRolesForPermissionsAsync(Id);
            result.Roles = roles;
            return result;
        }

       
    }
}
