using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BugTrakr.Models;
using BugTrakr.DTOs;
using BugTrakr.Services;
using BugTrakr.Controllers;
using System.Threading.Tasks;

namespace BugTrakr.Tests.Controllers;

// This class contains all the unit tests for the AuthController.
public class AuthControllerTests
{
    // A mock instance of the authentication service to simulate its behavior.
    private readonly Mock<IAuthService> _mockAuthService;
    // An instance of the controller to be tested.
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        // Arrange: Initialize the mock service and controller for each test.
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Register_Returns_Ok_WhenUserCreated()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "newuser", Email = "new@example.com", Password = "password", FirstName = "New", LastName = "User" };
        var createdUser = new User { UserID = 1, Username = registerDto.Username, Email = registerDto.Email, FirstName = registerDto.FirstName, LastName = registerDto.LastName, PasswordHash = "hashed", PasswordSalt = "salt" };

        _mockAuthService.Setup(service => service.RegisterAsync(registerDto))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Contains("Registration successful", okResult.Value?.ToString());
    }

    [Fact]
    public async Task Register_Returns_BadRequest_When_Username_Exists()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "existinguser", Email = "existing@example.com", Password = "password" };
        _mockAuthService.Setup(service => service.RegisterAsync(registerDto))
            .ReturnsAsync((User?)null); // Simulate a failed registration

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Username already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_Returns_Ok_With_Token_WhenCredentialsValid()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "validuser", Password = "password" };
        var fakeToken = "this-is-a-fake-jwt-token";
        _mockAuthService.Setup(service => service.LoginAsync(loginDto.Username, loginDto.Password))
            .ReturnsAsync(fakeToken);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var value = okResult.Value;
        var tokenValue = value?.GetType().GetProperty("Token")?.GetValue(value, null);

        Assert.Equal(fakeToken, tokenValue);
    }

    [Fact]
    public async Task Login_Returns_Unauthorized_For_Invalid_Credentials()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "invaliduser", Password = "wrongpassword" };
        _mockAuthService.Setup(service => service.LoginAsync(loginDto.Username, loginDto.Password))
            .ReturnsAsync((string?)null); // Simulate a failed login

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorizedResult.StatusCode);
        Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
    }
}
