using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugTrakr.DTOs;
using BugTrakr.Exceptions;
using BugTrakr.Models;
using BugTrakr.Repositories;
using BugTrakr.Services;
using Moq;
using Xunit;

namespace BugTrakr.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _mockTicketRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly TicketService _service;

        public TicketServiceTests()
        {
            _mockTicketRepository = new Mock<ITicketRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _service = new TicketService(
                _mockTicketRepository.Object,
                _mockProjectRepository.Object,
                _mockUserRepository.Object
            );
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldCreateTicket_WhenDataIsValid()
        {
            // Arrange
            var ticketDto = new CreateTicketDto
            {
                ProjectID = 1,
                ReporterID = 2,
                AssigneeID = 3,
                Title = "Sample",
                Description = "desc",
                Status = "Open"
            };

            var project = new Project { ProjectID = 1 };
            var reporter = new User
            {
                UserID = 1,
                Username = "reporter",
                Email = "reporter@example.com",
                FirstName = "Report",
                LastName = "Er",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            var assignee = new User
            {
                UserID = 3,
                Username = "assignee",
                Email = "assignee@example.com",
                FirstName = "Assign",
                LastName = "Ee",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };

            _mockProjectRepository.Setup(r => r.GetProjectByIdAsync(1)).ReturnsAsync(project);
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(2)).ReturnsAsync(reporter);
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(3)).ReturnsAsync(assignee);
            _mockTicketRepository.Setup(r => r.AddTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(new Ticket { });

            // Act
            var result = await _service.CreateTicketAsync(ticketDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ticketDto.ProjectID, result.ProjectID);
            Assert.Equal(ticketDto.Title, result.Title);
            Assert.Equal(ticketDto.Description, result.Description);
            Assert.Equal(ticketDto.Status, result.Status);
            Assert.Equal(ticketDto.ReporterID, result.ReporterID);
            Assert.Equal(ticketDto.AssigneeID, result.AssigneeID);
            Assert.Equal(project, result.Project);
            Assert.Equal(reporter, result.Reporter);
            Assert.Equal(assignee, result.Assignee);
            _mockTicketRepository.Verify(r => r.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
        {
            var ticketDto = new CreateTicketDto
            {
                ProjectID = 99,
                ReporterID = 2,
                Title = "Sample",
                Description = "desc"
            };

            _mockProjectRepository.Setup(r => r.GetProjectByIdAsync(99)).ReturnsAsync((Project?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateTicketAsync(ticketDto));
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldThrowNotFoundException_WhenReporterDoesNotExist()
        {
            var ticketDto = new CreateTicketDto
            {
                ProjectID = 1,
                ReporterID = 99,
                Title = "Sample",
                Description = "desc"
            };

            _mockProjectRepository.Setup(r => r.GetProjectByIdAsync(1)).ReturnsAsync(new Project());
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(99)).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateTicketAsync(ticketDto));
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldThrowNotFoundException_WhenAssigneeDoesNotExist()
        {
            var testUser = new User
            {
                UserID = 1,
                Username = "reporter",
                Email = "reporter@example.com",
                FirstName = "Report",
                LastName = "Er",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            var ticketDto = new CreateTicketDto
            {
                ProjectID = 1,
                ReporterID = 2,
                AssigneeID = 99,
                Title = "Sample",
                Description = "desc"
            };

            _mockProjectRepository.Setup(r => r.GetProjectByIdAsync(1)).ReturnsAsync(new Project());
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(2)).ReturnsAsync(testUser);
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(99)).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateTicketAsync(ticketDto));
        }

        [Fact]
        public async Task GetAllTicketsAsync_ShouldReturnTickets()
        {
            var tickets = new List<Ticket> { new Ticket(), new Ticket() };
            _mockTicketRepository.Setup(r => r.GetAllTicketsAsync()).ReturnsAsync(tickets);

            var result = await _service.GetAllTicketsAsync();

            Assert.Equal(2, ((List<Ticket>)result).Count);
        }

        [Fact]
        public async Task GetTicketByIdAsync_ShouldReturnTicket()
        {
            var ticket = new Ticket { TicketID = 42 };
            _mockTicketRepository.Setup(r => r.GetTicketByIdAsync(42)).ReturnsAsync(ticket);

            var result = await _service.GetTicketByIdAsync(42);

            Assert.NotNull(result);
            Assert.Equal(42, result.TicketID);
        }

        [Fact]
        public async Task UpdateTicketAsync_ShouldCallRepoUpdateMethods()
        {
            var ticket = new Ticket { TicketID = 1 };
            _mockTicketRepository.Setup(r => r.UpdateTicketAsync(ticket)).Returns(Task.CompletedTask);
            _mockTicketRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _service.UpdateTicketAsync(ticket);

            _mockTicketRepository.Verify(r => r.UpdateTicketAsync(ticket), Times.Once);
            _mockTicketRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteTicketAsync_ShouldCallRepoDeleteMethods()
        {
            _mockTicketRepository.Setup(r => r.DeleteTicketAsync(1)).Returns(Task.CompletedTask);
            _mockTicketRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _service.DeleteTicketAsync(1);

            _mockTicketRepository.Verify(r => r.DeleteTicketAsync(1), Times.Once);
            _mockTicketRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}