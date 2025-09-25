using BugTrakr.Models;
using BugTrakr.Repositories;
using BugTrakr.Services;
using BugTrakr.Exceptions;

namespace BugTrakr.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    public ProjectService(IProjectRepository projectRepo, IUserRepository userRepository, IProjectMemberRepository projectMemberRepository)
    {
        _projectRepository = projectRepo;
        _userRepository = userRepository;
        _projectMemberRepository = projectMemberRepository;
    }

    public async Task AddProjectAsync(Project project)
    {
        await _projectRepository.AddProjectAsync(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllProjectsAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetProjectByIdAsync(id);
    }

    public async Task UpdateProjectAsync(Project project)
    {
        await _projectRepository.UpdateProjectAsync(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteProjectAsync(id);
        await _projectRepository.SaveChangesAsync();
    }

    /// <summary>
        /// Adds a user as a member to a project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="userId">The ID of the user to add.</param>
        /// <returns>The newly created ProjectMember entity.</returns>
        public async Task<ProjectMember> AddMemberToProjectAsync(int projectId, int userId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {projectId} not found.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            if (await _projectMemberRepository.IsMemberAsync(projectId, userId))
            {
                throw new InvalidOperationException($"User {user.Username} is already a member of project {project.Name}.");
            }

            var projectMember = new ProjectMember
            {
                ProjectID = projectId,
                UserID = userId
            };

            await _projectMemberRepository.AddMemberAsync(projectMember);
            return projectMember;
        }
    
}
