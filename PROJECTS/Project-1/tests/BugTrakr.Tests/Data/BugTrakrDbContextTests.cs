using BugTrakr.Data;
using BugTrakr.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace BugTrakr.Tests.Data
{
    public class BugTrakrDbContextTests
    {
        private BugTrakrDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<BugTrakrDbContext>()
                .UseInMemoryDatabase(databaseName: "BugTrakrTestDb")
                .Options;
            return new BugTrakrDbContext(options);
        }

        [Fact]
        public void BugTrakrDbContext_Has_Expected_DbSets()
        {
            var context = CreateDbContext();

            context.Users.Should().NotBeNull();
            context.Projects.Should().NotBeNull();
            context.Tickets.Should().NotBeNull();
            context.ProjectMembers.Should().NotBeNull();
        }

        [Fact]
        public void ProjectMember_Has_Composite_Key()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(ProjectMember));
            var key = entityType?.FindPrimaryKey();

            key?.Properties.Should().Contain(p => p.Name == "ProjectID");
            key?.Properties.Should().Contain(p => p.Name == "UserID");
            key?.Properties.Should().HaveCount(2);
        }

        [Fact]
        public void ProjectMember_User_Relationship_Is_Correct()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(ProjectMember));
            var fk = entityType?.GetForeignKeys().Single(fk => fk.Properties.Any(p => p.Name == "UserID"));

            fk?.PrincipalEntityType.ClrType.Should().Be(typeof(User));
            fk?.PrincipalToDependent?.Name.Should().Be("ProjectMemberships");
        }

        [Fact]
        public void ProjectMember_Project_Relationship_Is_Correct()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(ProjectMember));
            var fk = entityType?.GetForeignKeys().Single(fk => fk.Properties.Any(p => p.Name == "ProjectID"));

            fk?.PrincipalEntityType.ClrType.Should().Be(typeof(Project));
            fk?.PrincipalToDependent?.Name.Should().Be("ProjectMembers");
        }

        [Fact]
        public void Ticket_Project_Relationship_Is_Cascade_Delete()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(Ticket));
            var fk = entityType?.GetForeignKeys().Single(fk => fk.Properties.Any(p => p.Name == "ProjectID"));

            fk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
        }

        [Fact]
        public void Ticket_Reporter_Relationship_Is_NoAction()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(Ticket));
            var fk = entityType?.GetForeignKeys().Single(fk => fk.Properties.Any(p => p.Name == "ReporterID"));

            fk?.DeleteBehavior.Should().Be(DeleteBehavior.NoAction);
        }

        [Fact]
        public void Ticket_Assignee_Relationship_Is_NoAction()
        {
            var context = CreateDbContext();
            var entityType = context.Model.FindEntityType(typeof(Ticket));
            var fk = entityType?.GetForeignKeys().Single(fk => fk.Properties.Any(p => p.Name == "AssigneeID"));

            fk?.DeleteBehavior.Should().Be(DeleteBehavior.NoAction);
        }
    }
}