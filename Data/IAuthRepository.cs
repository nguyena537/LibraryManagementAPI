using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(Member member, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
