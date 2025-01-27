using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task4.Services;
using System;
using Task4.Models;

namespace Task4.Filters
{
    public class BlockStatusFilter : IAsyncResultFilter
    {
        private readonly IUserStatusService _userStatusService;
        private readonly SignInManager<User> _signInManager;

        public BlockStatusFilter(IUserStatusService userStatusService, SignInManager<User> signInManager)
        {
            _userStatusService = userStatusService;
            _signInManager = signInManager;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var userId = _signInManager.Context.User?.Identity?.Name;

            if (userId == null)
            {
                await next();
                return;
            }

            bool isBlocked = await _userStatusService.IsUserBlockedAsync(userId);
            var currentUrl = context.HttpContext.Request.Path;

            if (isBlocked && !currentUrl.StartsWithSegments("/Account/Login"))
            {
                context.Result = new LocalRedirectResult("~/Account/Login");
                return;
            }

            await next();
        }
    }
}
