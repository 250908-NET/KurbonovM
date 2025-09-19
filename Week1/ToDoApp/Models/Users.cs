using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public List<TodoItem> TodoItems { get; set; } = new();
}