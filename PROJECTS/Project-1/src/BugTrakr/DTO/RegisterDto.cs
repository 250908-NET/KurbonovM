using System.ComponentModel.DataAnnotations;

namespace BugTrakr.DTOs
{
    // Data Transfer Object for user registration.
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;
    }
}
