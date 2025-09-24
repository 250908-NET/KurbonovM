using Microsoft.EntityFrameworkCore;
using BugTrakr.Models;
using BugTrakr.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers;

[ApiController]
[Route("api/users")]
public class UserService : ControllerBase
{
    private readonly IUserService _userService;
    public UserService(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        await _userService.AddUserAsync(user);
        return Created($"/users/{user.UserID}", user);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User updatedUser)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser is null)
        {
            return NotFound();
        }
        existingUser.Username = updatedUser.Username;
        existingUser.Email = updatedUser.Email;
        await _userService.UpdateUserAsync(existingUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser is null)
        {
            return NotFound();
        }
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}