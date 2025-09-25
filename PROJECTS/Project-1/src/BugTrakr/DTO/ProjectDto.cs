
using System.ComponentModel.DataAnnotations;

namespace BugTrakr.DTOs;

public class ProjectDto
{
    public int ProjectID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // This is where we include the related data
    public ICollection<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    public ICollection<ProjectMemberDto> ProjectMembers { get; set; } = new List<ProjectMemberDto>();
}

    public class TicketDto
    {
        public int TicketID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ReporterID { get; set; }
        public int? AssigneeID { get; set; }
    }

        public class ProjectMemberDto
    {
        public int UserID { get; set; }
    }