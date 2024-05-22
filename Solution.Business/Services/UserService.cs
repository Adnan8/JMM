using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solution.Business.Services.IServices;
using Solution.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Solution.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Solution.Repository.Repo.IRepo;
using HashidsNet;
using Solution.Common;
using Solution.Repository.Repo;
using static Azure.Core.HttpHeader;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.ComponentModel.Design;
using Solution.Business.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Solution.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitofWork _unitofWork;
        private readonly IConfiguration _configuration;
        private readonly Hashids _hashids;
        private readonly ICommonService _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IUnitofWork unitofWork, ICommonService common, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _hashids = new Hashids(ConstantUnique.HashidsName, ConstantUnique.HashidsLength);
            _roleManager = roleManager;
            _unitofWork = unitofWork;
            _common = common;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IdentityResultExtend> RegisterUserAsync(UserVM model)
        {
            var resultExtension = new IdentityResultExtend();

            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    resultExtension.Result = IdentityResult.Failed(new IdentityError { Description = $"Email '{model.Email}' is already taken." });
                    return resultExtension;
                }

                //var decodedCompId = string.IsNullOrEmpty(model.CompId) ? new int[0] : _hashids.Decode(model.CompId);
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    CompId = int.TryParse(model.CompId, out int parsedId) ? parsedId : 0,
                    UserType = model.UserType.ToString()
                };

                var createResult = await _userManager.CreateAsync(newUser, model.Password);
                if (!createResult.Succeeded)
                {
                    resultExtension.Result = createResult;
                    return resultExtension;
                }

                resultExtension.UserId = newUser.Id;

                if (!string.IsNullOrEmpty(model.RoleId))
                {
                    var roleAddedResult = await AddUserToRole(newUser, model.RoleId);
                    if (!roleAddedResult.Succeeded)
                    {
                        resultExtension.Result = roleAddedResult;
                        return resultExtension;
                    }
                }

                resultExtension.Result = IdentityResult.Success;
            }
            catch (Exception ex)
            {
                resultExtension.Result = IdentityResult.Failed(new IdentityError { Description = $"Internal server error: {ex.Message}" });
            }

            return resultExtension;
        }
        private async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleId)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleId);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleId));
            }

            return await _userManager.AddToRoleAsync(user, roleId);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
        public async Task<LoginResponseVM> LoginUserAsync(UserVM model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                var userClaims = await _userManager.GetClaimsAsync(user);
                var ipAddress = GetIpAddress();

                await RecordLoginHistory(user.Id, ipAddress);

                string companyId = "0", companyName = "";
                if (roles.Any(role => "User".Equals(role, StringComparison.OrdinalIgnoreCase) || "SuperAdmin".Equals(role, StringComparison.OrdinalIgnoreCase) || "Admin".Equals(role, StringComparison.OrdinalIgnoreCase)))
                {
                    if (user.CompId.HasValue)
                    {
                        companyId = user.CompId.Value.ToString();
                        companyName = await GetCompanyName(companyId) ?? string.Empty;
                    }
                }

                var claims = new List<Claim>
        {
            new Claim(CommonClaims.CompId, companyId),
            new Claim(CommonClaims.CompName, companyName),
            new Claim(CommonClaims.UserId, user.Id),
            new Claim(CommonClaims.UserEmail, user.Email)
        };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                claims.AddRange(userClaims);

                await RefreshSignInAsync(user, claims); // Re-sign in to apply the new claims

                var userRoles = await GetUserRolesWithMenusAsync(user.Id, companyId);
                return new LoginResponseVM
                {
                    UserId = user.Id,
                    CompanyId = companyId,
                    RoleData = userRoles,
                };
            }

            return HandleLoginFailure(result);
        }

        private string GetIpAddress()
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (IPAddress.IsLoopback(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress))
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        private async Task RecordLoginHistory(string userId, string ipAddress)
        {
            var loginHistory = new LoginHistory
            {
                UserId = userId,
                LoginTime = DateTime.UtcNow,
                IPAddress = ipAddress
            };
            await _unitofWork.LoginHistoryRepository.Insert(loginHistory);
        }

        private async Task<string> GetCompanyName(string companyId)
        {
            return _unitofWork.CompanyRepository.All
                    .Where(x => x.Id == Convert.ToInt32(companyId))
                    .Select(x => x.Compname)
                    .FirstOrDefault();
        }

        private async Task RefreshSignInAsync(ApplicationUser user, List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: true, authenticationMethod: CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private LoginResponseVM HandleLoginFailure(SignInResult result)
        {
            if (result.IsLockedOut)
                return new LoginResponseVM { LoginResult = LoginResult.LockedOut };
            if (result.IsNotAllowed)
                return new LoginResponseVM { LoginResult = LoginResult.NotAllowed };
            if (result.RequiresTwoFactor)
                return new LoginResponseVM { LoginResult = LoginResult.RequiresTwoFactor };

            return new LoginResponseVM { LoginResult = LoginResult.WrongPassword };
        }

        //public async Task<LoginResponseVM> LoginUserAsync(UserVM model)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        //    if (result.Succeeded)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        var roles = await _userManager.GetRolesAsync(user);
        //        var userClaims = await _userManager.GetClaimsAsync(user);
        //        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        //        if (IPAddress.IsLoopback(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress))
        //        {
        //            ipAddress = "127.0.0.1";
        //        }

        //        var loginHistory = new LoginHistory
        //        {
        //            UserId = user.Id,
        //            LoginTime = DateTime.UtcNow,
        //            IPAddress = ipAddress
        //        };
        //        await _unitofWork.LoginHistoryRepository.Insert(loginHistory);
        //        string companyId;
        //        string CompanyNAme;
        //        if (roles.Any(role => role.Equals("User", StringComparison.OrdinalIgnoreCase) || role.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)|| role.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
        //        {
        //            companyId = user.CompId.ToString();
        //            CompanyNAme = _unitofWork.CompanyRepository.All.Where(x => x.Id == Convert.ToInt32(companyId)).Select(x => x.Compname).FirstOrDefault();
        //        }
        //        else
        //        {
        //            companyId = "0";
        //            CompanyNAme = "";
        //        }

        //        var claims = new List<Claim>
        //        {
        //            new Claim(CommonClaims.CompId, companyId),
        //            new Claim(CommonClaims.CompName, CompanyNAme),
        //            new Claim(CommonClaims.UserId, user.Id),
        //            new Claim(CommonClaims.UserEmail, user.Email)
        //        };

        //        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        //        claims.AddRange(userClaims);
        //        var userRoles = await GetUserRolesWithMenusAsync(user.Id, companyId);
        //        return new LoginResponseVM
        //        {
        //            UserId = user.Id,
        //            CompanyId = companyId,
        //            RoleData = userRoles,
        //        };
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return new LoginResponseVM { LoginResult = LoginResult.LockedOut };
        //    }

        //    if (result.IsNotAllowed)
        //    {
        //        return new LoginResponseVM { LoginResult = LoginResult.NotAllowed };
        //    }

        //    if (result.RequiresTwoFactor)
        //    {
        //        return new LoginResponseVM { LoginResult = LoginResult.RequiresTwoFactor };
        //    }

        //    return new LoginResponseVM { LoginResult = LoginResult.WrongPassword };

        //    return null;
        //}

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle the case when the user is not found
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result;
        }
        public async Task<IEnumerable<UserVM>> GetAllAsync()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            
            var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var currentUser = await _userManager.FindByEmailAsync(emailClaim);
            bool isEnterprise = currentUser.CompId == 0;
            var filteredUsers = new List<UserVM>();
            if (isEnterprise)
            {
                filteredUsers = _userManager.Users
                    .Where(u => u.Id != currentUser.Id)
                    .Select(u => new UserVM
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        DisplayName = u.DisplayName,
                        PortalId = u.PortalId.ToString(),
                        Email = u.Email
                    }).ToList();
            }
            else
            {
                filteredUsers = _userManager.Users
                    .Where(u => u.Id != currentUser.Id && u.CompId == currentUser.CompId)
                    .Select(u => new UserVM
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        DisplayName = u.DisplayName,
                        PortalId = u.PortalId.ToString(),
                        Email = u.Email
                    }).ToList();
            }
            return filteredUsers;
        }



        public async Task<bool> UpdateAsync(UserVM updatedUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updatedUser.Id);
                if (user == null)
                {
                    return false;
                }
                user.UserName = updatedUser.UserName;
                user.DisplayName = updatedUser.DisplayName;
                user.Email = updatedUser.Email;
                //user.PhoneNumber = updatedUser.PhoneNumber;
                if (updatedUser.ProfileImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        //await updatedUser.ProfileImage.CopyToAsync(memoryStream);
                        //user.ProfileImage = memoryStream.ToArray();
                    }
                }
                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<UserRoleMenusVM>> GetUserRolesWithMenusAsync(string userId, string CompanyId)
        {
            var userRolesWithMenus = new List<UserRoleMenusVM>();
            if (CompanyId == "0")
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new List<UserRoleMenusVM>();
                }

                var roleNames = await _userManager.GetRolesAsync(user);

                if (!roleNames.Any())
                {
                    var defaultMenus = _unitofWork.MenuRepository.All.Where(x => x.IsDeleted == null || x.IsDeleted == false).Select(d => _common.Map<MenuVM>(d)).ToList();
                    var defaultPermissions = _unitofWork.PermissionRepository.All.Where(x => x.IsDeleted == null || x.IsDeleted == false).Select(d => _common.Map<PermissionVM>(d)).ToList();

                    userRolesWithMenus.Add(new UserRoleMenusVM
                    {
                        RoleId = "Enterprise",
                        RoleName = "Enterprise",
                        RoleMenus = defaultMenus,
                        RolePermissions = defaultPermissions
                    });
                }
                else
                {
                    foreach (var roleName in roleNames)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            var menus = await GetMenusByRoleIdAsync(role.Id);
                            var permissions = await GetPermissionsByRoleIdAsync(role.Id);

                            userRolesWithMenus.Add(new UserRoleMenusVM
                            {
                                RoleId = role.Id,
                                RoleName = role.Name,
                                RoleMenus = menus,
                                RolePermissions = permissions
                            });
                        }
                    }
                }
            }
            else
            {

                var Company = _unitofWork.CompanyRepository.All.Where(x => x.Id == Convert.ToInt32(CompanyId)).FirstOrDefault();
                if (Company != null && Company.DefaultPermissions != null && Company.DefaultPermissions == true)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        return new List<UserRoleMenusVM>();
                    }

                    var roleNames = await _userManager.GetRolesAsync(user);

                    if (!roleNames.Any())
                    {
                        var defaultMenus = _unitofWork.MenuRepository.All.Select(d => _common.Map<MenuVM>(d)).ToList();
                        var defaultPermissions = _unitofWork.PermissionRepository.All.Select(d => _common.Map<PermissionVM>(d)).ToList();

                        userRolesWithMenus.Add(new UserRoleMenusVM
                        {
                            RoleId = "Enterprise",
                            RoleName = "Enterprise",
                            RoleMenus = defaultMenus,
                            RolePermissions = defaultPermissions
                        });
                    }
                    else
                    {
                        foreach (var roleName in roleNames)
                        {
                            var role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                var menus = await GetMenusByRoleIdAsync(role.Id);
                                var permissions = await GetPermissionsByRoleIdAsync(role.Id);

                                userRolesWithMenus.Add(new UserRoleMenusVM
                                {
                                    RoleId = role.Id,
                                    RoleName = role.Name,
                                    RoleMenus = menus,
                                    RolePermissions = permissions
                                });
                            }
                        }
                    }
                }
            }

            return userRolesWithMenus;
        }
        private async Task<List<MenuVM>> GetMenusByRoleIdAsync(string roleId)
        {
            var menus = await _unitofWork.RoleMenuRepository.All
                .Where(rm => rm.RoleId == roleId && (rm.IsDeleted == false || rm.IsDeleted == null))
                .Join(_unitofWork.MenuRepository.All.Where(x => x.IsDeleted == null || x.IsDeleted == false),
                      roleMenu => roleMenu.MenuId,
                      menu => menu.Id,
                      (roleMenu, menu) => new MenuVM
                      {
                          Id = menu.Id.ToString(),
                          Title = menu.Title,
                          Descr = menu.Descr,
                          ParentId = menu.ParentId,
                          Icon = menu.Icon,
                          Url = menu.Url,
                          MenuOrder = menu.MenuOrder,
                          Controller = menu.Controller,
                          Page = menu.Page
                      })
                .ToListAsync();

            return menus;
        }
        private async Task<List<PermissionVM>> GetPermissionsByRoleIdAsync(string roleId)
        {
            var menus = await _unitofWork.RolePermissionRepository.All
                .Where(rm => rm.RoleId == roleId && (rm.IsDeleted == null || rm.IsDeleted == false))
                .Join(_unitofWork.PermissionRepository.All,
                      rolePermission => rolePermission.PermissionId,
                      permission => permission.Id,
                      (rolepermission, permission) => new PermissionVM
                      {
                          Id = permission.Id.ToString(),
                          PermissionName = permission.PermissionName,
                          Controller = permission.Controller,
                          Action = permission.Action,
                      })
                .ToListAsync();

            return menus;
        }
        public async Task<UserProfileVM> GetUserProfileAsync(string userId)
        {
            var userProfile = new UserProfileVM();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            userProfile.DisplayName = user.DisplayName;
            userProfile.Email = user.Email;
            userProfile.Username = user.UserName;

            var companyId = user.CompId;
            if (companyId > 0)
            {
                var company = await _unitofWork.CompanyRepository.All
                    .Where(x => x.Id == companyId && (x.IsDeleted == null || x.IsDeleted == false))
                    .FirstOrDefaultAsync();

                if (company != null)
                {
                    userProfile.CompanyName = company.Compname;
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            userProfile.RoleId = roles.FirstOrDefault();

            DocumentType documentType = DocumentType.Other;
            if (roles.Contains("SuperAdmin"))
            {
                documentType = DocumentType.CompanyLogo;
            }
            else if (roles.Contains("User"))
            {
                documentType = DocumentType.ProfilePicture;
            }

            var profileImagePath = await GetProfileImagePath(userId, documentType);
            var webRootPath = _hostingEnvironment.WebRootPath;
            var relativePath = profileImagePath.Replace(webRootPath, "").Replace("\\", "/");
            var fullPath = Path.Combine(webRootPath, profileImagePath.TrimStart('\\', '/'));

            if (File.Exists(fullPath))
            {
                var imageBytes = await File.ReadAllBytesAsync(fullPath);
                userProfile.Image = Convert.ToBase64String(imageBytes);
            }
            userProfile.Path = profileImagePath.Replace("\\", "/");
            if (!userProfile.Path.StartsWith("/"))
            {
                userProfile.Path = "/" + userProfile.Path;
            }

            return userProfile;
        }

        private async Task<string> GetProfileImagePath(string userId, DocumentType documentType)
        {
            var document = await _unitofWork.DocumentRepository.All
                    .Where(d => d.DocumentType == documentType)
                    .OrderByDescending(d => d.CreatedOn)
                    .FirstOrDefaultAsync();

            if (document != null)
            {
                return document.Path;
            }
            else
            {
                return "";
            }
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(UserProfileVM model)
        {
            try
            {
                var userId = "";
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                user.DisplayName = model.DisplayName;
                var updateUserResult = await _userManager.UpdateAsync(user);
                //if (!updateUserResult.Succeeded)
                //{
                //    return updateUserResult;
                //}

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    DocumentType documentType = DocumentType.ProfilePicture;

                    if (userRoles.Contains("SuperAdmin"))
                    {
                        documentType = DocumentType.CompanyLogo;
                    }
                    else if (userRoles.Contains("User"))
                    {
                        documentType = DocumentType.ProfilePicture;
                    }
                    else
                    {
                        documentType = DocumentType.Other;
                    }
                    var documentSaveResult = await SaveDocumentAsync(model.ProfileImage, userId, documentType);
                    if (!documentSaveResult)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Failed to save document." });
                    }
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Log the exception details here using your logging framework
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred while updating the user profile: {ex.Message}" });
            }
        }


        private async Task<bool> SaveDocumentAsync(IFormFile file, string userId, DocumentType documentType)
        {
            try
            {
                //var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var userFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "documents", userId);

                Console.WriteLine($"Attempting to save file to: {userFolder}"); // Use your logging mechanism here

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }
                var filePath = Path.Combine(userFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var document = new DocumentVM
                {
                    DocumentType = documentType,
                    Path = filePath,
                    OriginalFileName = file.FileName,
                    Size = file.Length,
                    ContentType = file.ContentType,
                };
                var documentModel = _common.Map<Document>(document);
                await _unitofWork.DocumentRepository.Insert(documentModel);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving document: {ex.Message}"); // Use your logging mechanism here
                return false;
            }
        }



    }
}
