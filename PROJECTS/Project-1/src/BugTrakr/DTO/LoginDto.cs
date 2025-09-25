
using System.ComponentModel.DataAnnotations;

namespace BugTrakr.DTOs
{
    // Data Transfer Object for user login.
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
