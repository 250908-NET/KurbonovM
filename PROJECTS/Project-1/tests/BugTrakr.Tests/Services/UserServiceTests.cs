using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Models;
using BugTrakr.Repositories;
using BugTrakr.Services;
using Moq;
using Xunit;

namespace BugTrakr.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task AddUserAsync_CallsRepositoryMethods()
        {
            var user = new User
            {
                UserID = 1,
                Username = "reporter",
                Email = "reporter@example.com",
                FirstName = "Report",
                LastName = "Er",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };

            _userRepoMock.Setup(r => r.AddUserAsync(user)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.AddUserAsync(user);

            _userRepoMock.Verify(r => r.AddUserAsync(user), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var user1 = new User
            {
                UserID = 1,
                Username = "reporter",
                Email = "reporter@example.com",
                FirstName = "Report",
                LastName = "Er",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            var user2 = new User
            {
                UserID = 2,
                Username = "reporter1",
                Email = "reporte1r@example.com",
                FirstName = "Report1",
                LastName = "Er1",
                PasswordHash = "hash1",
                PasswordSalt = "salt1"
            };
            var users = new List<User> { user1, user2 };
            _userRepoMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.Equal(2, result.Count());
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserById()
        {
            var user = new User
            {
                UserID = 5,
                Username = "reporter5",
                Email = "reporter5@example.com",
                FirstName = "Report5",
                LastName = "Er5",
                PasswordHash = "hash5",
                PasswordSalt = "salt5"
            };
            _userRepoMock.Setup(r => r.GetUserByIdAsync(5)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(5);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsUserByUsername()
        {
            var user = new User
            {
                UserID = 10,
                Username = "reporter10",
                Email = "reporter10@example.com",
                FirstName = "Report10",
                LastName = "Er10",
                PasswordHash = "hash10",
                PasswordSalt = "salt10"
            };
            _userRepoMock.Setup(r => r.GetUserByUsernameAsync("testuser")).ReturnsAsync(user);

            var result = await _userService.GetUserByUsernameAsync("testuser");

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsRepositoryMethods()
        {
            var user = new User
            {
                UserID = 11,
                Username = "reporter11",
                Email = "reporter11@example.com",
                FirstName = "Report11",
                LastName = "Er11",
                PasswordHash = "hash11",
                PasswordSalt = "salt11"
            };

            _userRepoMock.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.UpdateUserAsync(user);

            _userRepoMock.Verify(r => r.UpdateUserAsync(user), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsRepositoryMethods()
        {
            _userRepoMock.Setup(r => r.DeleteUserAsync(11)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.DeleteUserAsync(11);

            _userRepoMock.Verify(r => r.DeleteUserAsync(11), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}