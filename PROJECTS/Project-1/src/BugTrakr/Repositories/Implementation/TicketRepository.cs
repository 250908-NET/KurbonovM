using BugTrakr.Models;
using BugTrakr.Data;
using Microsoft.EntityFrameworkCore;

namespace BugTrakr.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly BugTrakrDbContext _context;

        public TicketRepository(BugTrakrDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await GetTicketByIdAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}