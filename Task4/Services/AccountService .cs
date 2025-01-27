using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Task4.Models;
using Task4.ViewModels;

namespace Task4.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            User user = new User
            {
                UserName = model.Login,
                Email = model.Email,
                FullName = model.FullName,
                RegistrationDate = DateTime.Now,
                LastLoginTime = DateTime.Now,
                IsEnable = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }
    }
}
