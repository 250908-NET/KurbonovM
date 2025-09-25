using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BugTrakr.Models;

// Represents a project that contains multiple tickets.
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    [JsonIgnore]
    public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}