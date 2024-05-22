using Microsoft.AspNetCore.Identity;
using Solution.Common.ViewModel;
using Solution.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResultExtend> RegisterUserAsync(UserVM model);
        Task<LoginResponseVM> LoginUserAsync(UserVM model);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IEnumerable<UserVM>> GetAllAsync();
        public Task<bool> DeleteAsync(string id);
        public Task<bool> UpdateAsync(UserVM updatedUser);
        Task<IEnumerable<UserRoleMenusVM>> GetUserRolesWithMenusAsync(string userId, string CompanyId);
        Task<UserProfileVM> GetUserProfileAsync(string userId);
        Task<IdentityResult> UpdateUserProfileAsync(UserProfileVM model);
    }
}
