using System;
using System.Text.Json.Serialization;
namespace BugTrakr.Models;

// A join table for the many-to-many relationship between Users and Projects.
public class ProjectMember
{
    public int ProjectID { get; set; }

    public int UserID { get; set; }
    [JsonIgnore]
    public Project Project { get; set; } = null!;
    [JsonIgnore]
    public User User { get; set; } = null!;
}