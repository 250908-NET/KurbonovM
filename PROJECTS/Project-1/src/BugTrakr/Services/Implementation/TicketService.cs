using BugTrakr.Exceptions;
using BugTrakr.Models;
using BugTrakr.Repositories;
using BugTrakr.Services;
using BugTrakr.DTOs;
using System.Threading.Tasks;

namespace BugTrakr.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(ITicketRepository ticketRepository, IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

        // Creates a new ticket from a DTO.
    public async Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto)
    {
        // Fetch related entities from the database
        var project = await _projectRepository.GetProjectByIdAsync(ticketDto.ProjectID);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {ticketDto.ProjectID} not found.");
        }

        var reporter = await _userRepository.GetUserByIdAsync(ticketDto.ReporterID);
        if (reporter == null)
        {
            throw new NotFoundException($"Reporter with ID {ticketDto.ReporterID} not found.");
        }

        User? assignee = null;
        if (ticketDto.AssigneeID.HasValue)
        {
            assignee = await _userRepository.GetUserByIdAsync(ticketDto.AssigneeID.Value);
            if (assignee == null)
            {
                throw new NotFoundException($"Assignee with ID {ticketDto.AssigneeID.Value} not found.");
            }
        }
        
        // Build the full Ticket entity from the DTO and fetched entities
        var ticket = new Ticket
        {
            ProjectID = ticketDto.ProjectID,
            Project = project,
            Title = ticketDto.Title,
            Description = ticketDto.Description,
            Status = ticketDto.Status,
            ReporterID = ticketDto.ReporterID,
            Reporter = reporter,
            AssigneeID = ticketDto.AssigneeID,
            Assignee = assignee
        };

        await _ticketRepository.AddTicketAsync(ticket);
        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _ticketRepository.GetAllTicketsAsync();
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _ticketRepository.GetTicketByIdAsync(id);
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        await _ticketRepository.UpdateTicketAsync(ticket);
        await _ticketRepository.SaveChangesAsync();
    }

    public async Task DeleteTicketAsync(int id)
    {
        await _ticketRepository.DeleteTicketAsync(id);
        await _ticketRepository.SaveChangesAsync();
    }
    
}