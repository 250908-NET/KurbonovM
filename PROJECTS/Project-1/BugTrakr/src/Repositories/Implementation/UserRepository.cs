using BugTrakr.Models;
using BugTrakr.Data;
using Microsoft.EntityFrameworkCore;

namespace BugTrakr.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BugTrakrDbContext _context;
        public UserRepository(BugTrakrDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }
         public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await SaveChangesAsync();
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}