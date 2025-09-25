using BugTrakr.DTOs;
using BugTrakr.Models;

namespace BugTrakr.Services;

public interface IProjectService
{
    Task<Project?> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();

    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);

    // Adds a user as a member to a project.
    Task<ProjectMember> AddMemberToProjectAsync(int projectId, int userId);
}
