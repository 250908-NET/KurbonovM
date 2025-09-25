using System;

namespace BugTrakr.Models;

// A join table for the many-to-many relationship between Users and Projects.
public class ProjectMember
{
    public int ProjectID { get; set; }

    public int UserID { get; set; }

    public Project Project { get; set; } = null!;

    public User User { get; set; } = null!;
}