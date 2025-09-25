using BugTrakr.Models;
using BugTrakr.DTOs;

namespace BugTrakr.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterDto registerDto);
        Task<string?> LoginAsync(string username, string password);
    }
}