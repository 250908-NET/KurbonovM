using System.ComponentModel.DataAnnotations;

namespace BugTrakr.DTOs
{
    // Data Transfer Object for creating a new ticket.
    // It contains only the necessary properties sent from the client.
    public class CreateTicketDto
    {
        [Required]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        [Required]
        public int ReporterID { get; set; }

        public int? AssigneeID { get; set; }
    }
}
