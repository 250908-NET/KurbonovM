using BugTrakr.Models;
using BugTrakr.Repositories;


namespace BugTrakr.Services;
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task AddUserAsync(User user)
        {
            await _userRepo.AddUserAsync(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetUserByIdAsync(id);
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepo.GetUserByUsernameAsync(username);
        }
        public async Task UpdateUserAsync(User user)
        {
            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            await _userRepo.DeleteUserAsync(id);
            await _userRepo.SaveChangesAsync();
        }
    }
        