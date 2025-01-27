using Microsoft.AspNetCore.Identity;
using Task4.ViewModels;

public interface IAccountService
{
    Task<IdentityResult> RegisterAsync(RegisterViewModel model);
}