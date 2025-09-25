using BugTrakr.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTrakr.Repositories;
    // Defines the contract for data access operations related to ProjectMember entities.
    public interface IProjectMemberRepository
    {
        // Adds a new member to a project.
        Task<ProjectMember> AddMemberAsync(ProjectMember projectMember);

        // Checks if a user is a member of a project.
        Task<bool> IsMemberAsync(int projectId, int userId);
    }
