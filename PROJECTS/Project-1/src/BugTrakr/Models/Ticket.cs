using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BugTrakr.Models;

// Represents a single ticket or issue in the system.
public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TicketID { get; set; }

    public int ProjectID { get; set; }
    public Project Project { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int ReporterID { get; set; }
    public User Reporter { get; set; } = null!;

    public int? AssigneeID { get; set; }
    public User? Assignee { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}