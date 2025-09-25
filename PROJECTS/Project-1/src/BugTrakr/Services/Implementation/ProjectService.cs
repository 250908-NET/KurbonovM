using BugTrakr.Models;
using BugTrakr.Repositories;
using BugTrakr.Services;

namespace BugTrakr.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepo;

    public ProjectService(IProjectRepository projectRepo)
    {
        _projectRepo = projectRepo;
    }

    public async Task AddProjectAsync(Project project)
    {
        await _projectRepo.AddProjectAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _projectRepo.GetAllProjectsAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _projectRepo.GetProjectByIdAsync(id);
    }

    public async Task UpdateProjectAsync(Project project)
    {
        await _projectRepo.UpdateProjectAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepo.DeleteProjectAsync(id);
        await _projectRepo.SaveChangesAsync();
    }
    
}
