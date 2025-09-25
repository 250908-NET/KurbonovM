using BugTrakr.Models;

namespace BugTrakr.Services;

public interface IProjectService
{
    Task<Project?> GetProjectByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
}
