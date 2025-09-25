using BugTrakr.Models;

namespace BugTrakr.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket> AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task SaveChangesAsync();
    }
}