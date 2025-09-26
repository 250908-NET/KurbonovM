using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Data;
using BugTrakr.Models;
using BugTrakr.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BugTrakr.Tests.Repositories
{
    public class ProjectRepositoryTests
    {
        private BugTrakrDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BugTrakrDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BugTrakrDbContext(options);
        }

        private ProjectRepository GetRepository(BugTrakrDbContext context)
        {
            return new ProjectRepository(context);
        }

        [Fact]
        public async Task AddProjectAsync_ShouldAddProject()
        {
            // Arrange
            var context = GetDbContext();
            var repo = GetRepository(context);
            var project = new Project { ProjectID = 1, Name = "Test Project" };

            // Act
            await repo.AddProjectAsync(project);
            await repo.SaveChangesAsync();

            // Assert
            var result = await repo.GetProjectByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Test Project", result.Name);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);
            var project = new Project { ProjectID = 10, Name = "Sample Project" };
            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            var result = await repo.GetProjectByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("Sample Project", result.Name);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var result = await repo.GetProjectByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);
            await context.Projects.AddAsync(new Project { ProjectID = 1, Name = "P1" });
            await context.Projects.AddAsync(new Project { ProjectID = 2, Name = "P2" });
            await context.SaveChangesAsync();

            var result = await repo.GetAllProjectsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldUpdateProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);
            var project = new Project { ProjectID = 1, Name = "Initial" };
            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            project.Name = "Updated";
            await repo.UpdateProjectAsync(project);

            var result = await repo.GetProjectByIdAsync(1);
            Assert.Equal("Updated", result?.Name);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldRemoveProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);
            var project = new Project { ProjectID = 3, Name = "DeleteMe" };
            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            await repo.DeleteProjectAsync(3);
            await repo.SaveChangesAsync();

            var result = await repo.GetProjectByIdAsync(3);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProjectsWithDetailsAsync_ShouldIncludeRelatedEntities()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { ProjectID = 1, Name = "WithDetails" };
            project.Tickets = new List<Ticket> { new Ticket { ProjectID = 100, Title = "Bug" } };
            project.ProjectMembers = new List<ProjectMember> { new ProjectMember { ProjectID = 100, UserID = 200 } };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            var result = (await repo.GetAllProjectsWithDetailsAsync()).ToList();

            Assert.Single(result);
            Assert.Single(result[0].Tickets);
            Assert.Single(result[0].ProjectMembers);
            Assert.Equal("Bug", result[0].Tickets.First().Title);
            Assert.Equal(200, result[0].ProjectMembers.First().UserID);
        }
    }
}