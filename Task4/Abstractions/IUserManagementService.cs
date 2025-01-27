public interface IUserManagementService
{
    Task<bool> DeleteUserAsync(string userId);
    Task<bool> BlockUserAsync(string userId);
    Task<bool> UnblockUserAsync(string userId);
}