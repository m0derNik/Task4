using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Task4.Models;

namespace Task4.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly UserManager<User> _userManager;

        public UserStatusService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsUserBlockedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.IsEnable == false;
        }
    }
}
