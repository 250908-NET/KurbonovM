using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BugTrakr.Models;
using BugTrakr.Services;
using BugTrakr.Controllers;

namespace BugTrakr.Tests.Controllers;

// This class contains all the unit tests for the UserController.
public class UserControllerTests
{
    // A mock instance of the user service to simulate its behavior.
    private readonly Mock<IUserService> _mockUserService;
    // An instance of the controller to be tested.
    private readonly UserController _controller;
    // A list of sample users to be used in the tests.
    private readonly List<User> _sampleUsers = new List<User>
    {
        new User { UserID = 1, Username = "jdoe", Email = "jdoe@example.com" },
        new User { UserID = 2, Username = "jsmith", Email = "jsmith@example.com" }
    };

    public UserControllerTests()
    {
        // Arrange: Initialize the mock service and controller for each test.
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task GetAllUsers_Returns_Ok_With_Users()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetAllUsersAsync())
            .ReturnsAsync(_sampleUsers);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Equal(_sampleUsers.Count, returnedUsers.Count());
    }

    [Fact]
    public async Task GetUserById_Returns_Ok_With_User()
    {
        // Arrange
        var user = _sampleUsers.First();
        _mockUserService.Setup(service => service.GetUserByIdAsync(user.UserID))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(user.UserID);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user.UserID, returnedUser.UserID);
    }

    [Fact]
    public async Task GetUserById_Returns_NotFound_For_NonExistent_User()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetUserById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetUserByUsername_Returns_Ok_With_User()
    {
        // Arrange
        var user = _sampleUsers.First();
        _mockUserService.Setup(service => service.GetUserByUsernameAsync(user.Username))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserByUsername(user.Username);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user.Username, returnedUser.Username);
    }

    [Fact]
    public async Task GetUserByUsername_Returns_NotFound_For_NonExistent_User()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetUserByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetUserByUsername("nonexistent");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateUser_Returns_CreatedAt_On_Success()
    {
        // Arrange
        var newUser = new User { UserID = 1, Username = "newuser", Email = "new@example.com" };
        _mockUserService.Setup(service => service.AddUserAsync(newUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateUser(newUser);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"/users/{newUser.UserID}", createdResult.Location);
        Assert.Equal(newUser, createdResult.Value);
        _mockUserService.Verify(service => service.AddUserAsync(newUser), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_Returns_NoContent_On_Success()
    {
        // Arrange
        var userToUpdate = _sampleUsers.First();
        _mockUserService.Setup(service => service.GetUserByIdAsync(userToUpdate.UserID))
            .ReturnsAsync(userToUpdate);
        _mockUserService.Setup(service => service.UpdateUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateUser(userToUpdate.UserID, userToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockUserService.Verify(service => service.UpdateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_Returns_NotFound_For_NonExistent_User()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.UpdateUser(999, new User { Username = "nonexistent", Email = "test@example.com" });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteUser_Returns_NoContent_On_Success()
    {
        // Arrange
        var userToDelete = _sampleUsers.First();
        _mockUserService.Setup(service => service.GetUserByIdAsync(userToDelete.UserID))
            .ReturnsAsync(userToDelete);
        _mockUserService.Setup(service => service.DeleteUserAsync(userToDelete.UserID))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUser(userToDelete.UserID);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockUserService.Verify(service => service.DeleteUserAsync(userToDelete.UserID), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_Returns_NotFound_For_NonExistent_User()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.DeleteUser(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
