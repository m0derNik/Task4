using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Task4.Models;

namespace Task4.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<User> _userManager;

        public UserManagementService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> BlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnd = DateTime.Now.AddYears(200);
            user.IsEnable = false;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UnblockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnd = DateTime.Now;
            user.IsEnable = true;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
