using BugTrakr.DTOs;
using BugTrakr.Exceptions;
using BugTrakr.Models;
using BugTrakr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers;

[Authorize]
[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        await _projectService.AddProjectAsync(project);
        return Created($"/projects/{project.ProjectID}", project);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return project is not null ? Ok(project) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, Project updatedProject)
    {
        var existingProject = await _projectService.GetProjectByIdAsync(id);
        if (existingProject is null)
        {
            return NotFound();
        }
        existingProject.Name = updatedProject.Name;
        existingProject.Description = updatedProject.Description;
        await _projectService.UpdateProjectAsync(existingProject);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var existingProject = await _projectService.GetProjectByIdAsync(id);
        if (existingProject is null)
        {
            return NotFound();
        }
        await _projectService.DeleteProjectAsync(id);
        return NoContent();
    }

    // Adds a new member to a project.
    [HttpPost("add-member")]
    public async Task<IActionResult> AddMemberToProject([FromBody] AddMemberDto dto)
    {
        try
        {
            var projectMember = await _projectService.AddMemberToProjectAsync(dto.ProjectID, dto.UserID);
            return Ok(projectMember);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}