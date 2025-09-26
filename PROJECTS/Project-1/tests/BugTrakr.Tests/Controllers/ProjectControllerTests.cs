using BugTrakr.Controllers;
using BugTrakr.DTOs;
using BugTrakr.Exceptions;
using BugTrakr.Models;
using BugTrakr.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BugTrakr.Tests.Controllers;

public class ProjectControllerTests
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly ProjectController _controller;

    public ProjectControllerTests()
    {
        _mockProjectService = new Mock<IProjectService>();
        _controller = new ProjectController(_mockProjectService.Object);
    }

    [Fact]
    public async Task GetAllProjects_ReturnsOk_WithProjects()
    {
        // Arrange
        var projects = new List<ProjectDto> { new ProjectDto { ProjectID = 1, Name = "Test" } };
        _mockProjectService.Setup(s => s.GetAllProjectsAsync()).ReturnsAsync(projects);

        // Act
        var result = await _controller.GetAllProjects();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(projects, okResult.Value);
    }
  
    [Fact]
    public async Task CreateProject_CreatesProject_ReturnsCreated()
    {
        // Arrange
        var project = new Project { ProjectID = 1, Name = "New Project" };

        _mockProjectService.Setup(s => s.AddProjectAsync(project)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateProject(project);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"/projects/{project.ProjectID}", createdResult.Location);
        Assert.Equal(project, createdResult.Value);
    }

    [Fact]
    public async Task GetProjectById_ProjectExists_ReturnsOk()
    {
        // Arrange
        var project = new Project { ProjectID = 2, Name = "Existing Project" };
        _mockProjectService.Setup(s => s.GetProjectByIdAsync(2)).ReturnsAsync(project);

        // Act
        var result = await _controller.GetProjectById(2);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(project, okResult.Value);
    }

    [Fact]
    public async Task GetProjectById_ProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockProjectService.Setup(s => s.GetProjectByIdAsync(99)).ReturnsAsync((Project)null);

        // Act
        var result = await _controller.GetProjectById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateProject_ProjectExists_ReturnsNoContent()
    {
        // Arrange
        var existingProject = new Project { ProjectID = 3, Name = "Old Name", Description = "Old Desc" };
        var updatedProject = new Project { Name = "New Name", Description = "New Desc" };

        _mockProjectService.Setup(s => s.GetProjectByIdAsync(3)).ReturnsAsync(existingProject);
        _mockProjectService.Setup(s => s.UpdateProjectAsync(existingProject)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProject(3, updatedProject);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("New Name", existingProject.Name);
        Assert.Equal("New Desc", existingProject.Description);
    }

    [Fact]
    public async Task UpdateProject_ProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockProjectService.Setup(s => s.GetProjectByIdAsync(99)).ReturnsAsync((Project)null);

        // Act
        var result = await _controller.UpdateProject(99, new Project());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteProject_ProjectExists_ReturnsNoContent()
    {
        // Arrange
        var project = new Project { ProjectID = 4 };
        _mockProjectService.Setup(s => s.GetProjectByIdAsync(4)).ReturnsAsync(project);
        _mockProjectService.Setup(s => s.DeleteProjectAsync(4)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProject(4);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProject_ProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockProjectService.Setup(s => s.GetProjectByIdAsync(99)).ReturnsAsync((Project)null);

        // Act
        var result = await _controller.DeleteProject(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddMemberToProject_Success_ReturnsOk()
    {
        // Arrange
        var dto = new AddMemberDto { ProjectID = 1, UserID = 2 };
        var member = new ProjectMember { ProjectID = 1, UserID = 2 };

        _mockProjectService.Setup(s => s.AddMemberToProjectAsync(dto.ProjectID, dto.UserID)).ReturnsAsync(member);

        // Act
        var result = await _controller.AddMemberToProject(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(member, okResult.Value);
    }

    [Fact]
    public async Task AddMemberToProject_NotFoundException_ReturnsNotFound()
    {
        // Arrange
        var dto = new AddMemberDto { ProjectID = 1, UserID = 3 };
        _mockProjectService.Setup(s => s.AddMemberToProjectAsync(dto.ProjectID, dto.UserID))
            .ThrowsAsync(new NotFoundException("Project or user not found"));

        // Act
        var result = await _controller.AddMemberToProject(dto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Project or user not found", notFoundResult.Value);
    }

    [Fact]
    public async Task AddMemberToProject_InvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var dto = new AddMemberDto { ProjectID = 1, UserID = 3 };
        _mockProjectService.Setup(s => s.AddMemberToProjectAsync(dto.ProjectID, dto.UserID))
            .ThrowsAsync(new InvalidOperationException("User already a member"));

        // Act
        var result = await _controller.AddMemberToProject(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User already a member", badRequestResult.Value);
    }
}
