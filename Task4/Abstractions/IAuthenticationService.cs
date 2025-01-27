using Microsoft.AspNetCore.Identity;
using Task4.ViewModels;

public interface IAuthenticationService
{
    Task<SignInResult> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
}