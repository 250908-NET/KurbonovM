
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrakr.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    [StringLength(100)]
    public required string Email { get; set; }

    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }

    [Required]
    [StringLength(255)]
    public required string PasswordHash { get; set; }

    [Required]
    [StringLength(255)]
    public required string PasswordSalt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}