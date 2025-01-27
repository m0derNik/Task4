using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Task4.Models;
using Task4.Services;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IUserManagementService userManagementService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _userManagementService = userManagementService;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] userIds)
        {
            var failedDeletions = new List<string>();

            foreach (var userId in userIds)
            {
                var success = await _userManagementService.DeleteUserAsync(userId);
                if (!success)
                {
                    failedDeletions.Add(userId);
                }
            }

            if (failedDeletions.Any())
            {
                _logger.LogWarning("Failed to delete users with IDs: {UserIds}", string.Join(", ", failedDeletions));
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Block(string[] userIds)
        {
            var self = false;
            var failedBlocks = new List<string>();

            foreach (var userId in userIds)
            {
                var success = await _userManagementService.BlockUserAsync(userId);
                if (!success)
                {
                    failedBlocks.Add(userId);
                }
                else
                {
                    if (_userManager.GetUserId(User) == userId)
                    {
                        await _signInManager.SignOutAsync();
                        self = true;
                    }
                }
            }

            if (failedBlocks.Any())
            {
                _logger.LogWarning("Failed to block users with IDs: {UserIds}", string.Join(", ", failedBlocks));
            }

            return self ? RedirectToAction("Index", "Home") : RedirectToAction("Index");
        }

        public async Task<IActionResult> Unblock(string[] userIds)
        {
            var failedUnblocks = new List<string>();

            foreach (var userId in userIds)
            {
                var success = await _userManagementService.UnblockUserAsync(userId);
                if (!success)
                {
                    failedUnblocks.Add(userId);
                }
            }

            if (failedUnblocks.Any())
            {
                _logger.LogWarning("Failed to unblock users with IDs: {UserIds}", string.Join(", ", failedUnblocks));
            }

            return RedirectToAction("Index");
        }
    }
}
