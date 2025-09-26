using BugTrakr.Data;
using BugTrakr.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrakr.Repositories;
    // Implements the data access logic for the ProjectMember entity.
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly BugTrakrDbContext _context;

        public ProjectMemberRepository(BugTrakrDbContext context)
        {
            _context = context;
        }

        // Adds a new member to a project.
        public async Task<ProjectMember> AddMemberAsync(ProjectMember projectMember)
        {
            await _context.ProjectMembers.AddAsync(projectMember);
            await _context.SaveChangesAsync();
            return projectMember;
        }


        // Checks if a user is already a member of a project.
        public async Task<bool> IsMemberAsync(int projectId, int userId)
        {
            return await _context.ProjectMembers.AnyAsync(pm => pm.ProjectID == projectId && pm.UserID == userId);
        }
    }
