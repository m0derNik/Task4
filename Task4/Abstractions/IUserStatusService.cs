public interface IUserStatusService
{
    Task<bool> IsUserBlockedAsync(string userId);
}