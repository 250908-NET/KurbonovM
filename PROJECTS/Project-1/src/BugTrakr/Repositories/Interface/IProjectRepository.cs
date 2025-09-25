using BugTrakr.Models;

namespace BugTrakr.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task SaveChangesAsync();
    }
}