using System.Threading.Tasks;
using BugTrakr.Data;
using BugTrakr.Models;
using BugTrakr.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BugTrakr.Tests.Repositories
{
    public class ProjectMemberRepositoryTests
    {
        private BugTrakrDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BugTrakrDbContext>()
                .UseInMemoryDatabase(databaseName: "BugTrakrTestDb")
                .Options;
            return new BugTrakrDbContext(options);
        }

        [Fact]
        public async Task AddMemberAsync_AddsMemberToDb()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProjectMemberRepository(context);
            var member = new ProjectMember { ProjectID = 1, UserID = 1 };

            // Act
            var result = await repository.AddMemberAsync(member);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ProjectID);
            Assert.Equal(1, result.UserID);
            Assert.Contains(context.ProjectMembers, pm => pm.ProjectID == 1 && pm.UserID == 1);
        }

        [Fact]
        public async Task IsMemberAsync_ReturnsTrue_IfUserIsMember()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.ProjectMembers.Add(new ProjectMember { ProjectID = 1, UserID = 2 });
            await context.SaveChangesAsync();
            var repository = new ProjectMemberRepository(context);

            // Act
            var isMember = await repository.IsMemberAsync(1, 2);

            // Assert
            Assert.True(isMember);
        }

        [Fact]
        public async Task IsMemberAsync_ReturnsFalse_IfUserIsNotMember()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.ProjectMembers.Add(new ProjectMember { ProjectID = 1, UserID = 3 });
            await context.SaveChangesAsync();
            var repository = new ProjectMemberRepository(context);

            // Act
            var isMember = await repository.IsMemberAsync(1, 99);

            // Assert
            Assert.False(isMember);
        }
    }
}