using BugTrakr.Models;
using BugTrakr.Data;
using Microsoft.EntityFrameworkCore;
using BugTrakr.DTOs;

namespace BugTrakr.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BugTrakrDbContext _context;

        public ProjectRepository(BugTrakrDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsWithDetailsAsync()
        {
            // The .Include() method tells Entity Framework to load the related data
            // in a single, efficient query.
            return await _context.Projects
                .Include(p => p.Tickets)
                .Include(p => p.ProjectMembers)
                .ToListAsync();
        }

        public async Task AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await GetProjectByIdAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}