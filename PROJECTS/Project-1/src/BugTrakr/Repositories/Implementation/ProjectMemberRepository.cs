using BugTrakr.Data;
using BugTrakr.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrakr.Repositories;
    /// <summary>
    /// Implements the data access logic for the ProjectMember entity.
    /// </summary>
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly BugTrakrDbContext _context;

        public ProjectMemberRepository(BugTrakrDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new member to a project.
        /// </summary>
        /// <param name="projectMember">The ProjectMember entity to add.</param>
        /// <returns>The added ProjectMember entity.</returns>
        public async Task<ProjectMember> AddMemberAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
            await _context.SaveChangesAsync();
            return projectMember;
        }

        /// <summary>
        /// Checks if a user is already a member of a project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user is a member, otherwise false.</returns>
        public async Task<bool> IsMemberAsync(int projectId, int userId)
        {
            return await _context.ProjectMembers.AnyAsync(pm => pm.ProjectID == projectId && pm.UserID == userId);
        }
    }
