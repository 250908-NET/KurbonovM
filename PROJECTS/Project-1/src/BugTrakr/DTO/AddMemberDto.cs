
using System.ComponentModel.DataAnnotations;

namespace BugTrakr.DTOs
{
    // Data Transfer Object for adding a member to a project.
    public class AddMemberDto
    {
        [Required]
        public int ProjectID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
