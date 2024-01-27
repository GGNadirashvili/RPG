using RPGSln.Models;

namespace RPGSln.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(User user, string password);
        Task<bool> UserExists(string username);
    }
}
