using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BugTrakr.Models;
using BugTrakr.DTOs;
using BugTrakr.Services;
using BugTrakr.Controllers;
using System.Threading.Tasks;

namespace BugTrakr.Controllers;

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
    public async Task Register_Returns_Ok_On_Success()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "newuser", Email = "new@example.com", Password = "password", FirstName = "New", LastName = "User" };
        var createdUser = new User { UserID = 1, Username = registerDto.Username, Email = registerDto.Email, FirstName = registerDto.FirstName, LastName = registerDto.LastName, PasswordHash = "hashed", PasswordSalt = "salt" };

        _mockAuthService.Setup(service => service.RegisterAsync(registerDto))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
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
        Assert.IsType<BadRequestObjectResult>(result);
    }

    // [Fact]
    // public async Task Login_Returns_Ok_With_Token_On_Success()
    // {
    //     // Arrange
    //     var loginDto = new LoginDto { Username = "validuser", Password = "password" };
    //     var fakeToken = "this-is-a-fake-jwt-token";
    //     _mockAuthService.Setup(service => service.LoginAsync(loginDto.Username, loginDto.Password))
    //         .ReturnsAsync(fakeToken);

    //     // Act
    //     var result = await _controller.Login(loginDto);

    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     dynamic returnedObject = okResult.Value;
    //     Assert.Equal(fakeToken, returnedObject.Token);
    // }

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
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
