using BugTrakr.Models;
using BugTrakr.DTOs;
namespace BugTrakr.Services;

public interface ITicketService
{
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    //Task AddTicketAsync(Ticket ticket);
    Task UpdateTicketAsync(Ticket ticket);
    Task DeleteTicketAsync(int id);
    Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto);
}