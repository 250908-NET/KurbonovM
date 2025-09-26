using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Models;
using BugTrakr.Data;
using BugTrakr.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BugTrakr.Tests.Repositories;
public class TicketRepositoryTests
{
    private BugTrakrDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BugTrakrDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new BugTrakrDbContext(options);
    }
    private TicketRepository GetRepository(BugTrakrDbContext context)
    {
        return new TicketRepository(context);
    }

    private Ticket GetSampleTicket(int id = 1)
    {
        return new Ticket
        {
            TicketID = id,
            Title = "Sample Ticket",
            Description = "Sample Description",
            Status = "Open"
        };
    }

    [Fact]
    public async Task AddTicketAsync_ShouldAddTicket()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var ticket = GetSampleTicket();

        var result = await repo.AddTicketAsync(ticket);

        Assert.Equal(ticket, result);
        Assert.Equal(1, context.Tickets.Count());
    }

    [Fact]
    public async Task GetTicketByIdAsync_ShouldReturnTicket()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var ticket = GetSampleTicket();
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();

        var result = await repo.GetTicketByIdAsync(ticket.TicketID);

        Assert.NotNull(result);
        Assert.Equal(ticket.TicketID, result!.TicketID);
    }

    [Fact]
    public async Task GetTicketByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var result = await repo.GetTicketByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllTicketsAsync_ShouldReturnAllTickets()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var tickets = new List<Ticket>
        {
            GetSampleTicket(1),
            GetSampleTicket(2),
            GetSampleTicket(3)
        };
        await context.Tickets.AddRangeAsync(tickets);
        await context.SaveChangesAsync();

        var result = await repo.GetAllTicketsAsync();

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task UpdateTicketAsync_ShouldUpdateTicket()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var ticket = GetSampleTicket();
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();

        ticket.Title = "Updated Title";
        await repo.UpdateTicketAsync(ticket);

        var updated = await repo.GetTicketByIdAsync(ticket.TicketID);
        Assert.Equal("Updated Title", updated!.Title);
    }

    [Fact]
    public async Task DeleteTicketAsync_ShouldDeleteTicket()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var ticket = GetSampleTicket();
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();

        await repo.DeleteTicketAsync(ticket.TicketID);
        await repo.SaveChangesAsync();

        var deleted = await repo.GetTicketByIdAsync(ticket.TicketID);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChanges()
    {
        var context = GetDbContext();
        var repo = GetRepository(context);

        var ticket = GetSampleTicket();
        await context.Tickets.AddAsync(ticket);

        await repo.SaveChangesAsync();

        Assert.Equal(1, context.Tickets.Count());
    }
}
