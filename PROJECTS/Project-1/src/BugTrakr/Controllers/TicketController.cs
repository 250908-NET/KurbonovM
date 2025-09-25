using Microsoft.EntityFrameworkCore;
using BugTrakr.Models;
using BugTrakr.Services;
using BugTrakr.DTOs;
using BugTrakr.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticketDto)
        {
            try
            {
                var newTicket = await _ticketService.CreateTicketAsync(ticketDto);
                // In a real application, you would return the created resource's URL.
                return Ok(newTicket);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the ticket: " + ex.Message);
            }
        }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        return ticket is not null ? Ok(ticket) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, Ticket updatedTicket)
    {
        var existingTicket = await _ticketService.GetTicketByIdAsync(id);
        if (existingTicket is null)
        {
            return NotFound();
        }
        existingTicket.Title = updatedTicket.Title;
        existingTicket.Description = updatedTicket.Description;
        existingTicket.Status = updatedTicket.Status;
        existingTicket.AssigneeID = updatedTicket.AssigneeID;
        existingTicket.ReporterID = updatedTicket.ReporterID;
        existingTicket.ProjectID = updatedTicket.ProjectID;

        await _ticketService.UpdateTicketAsync(existingTicket);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var existingTicket = await _ticketService.GetTicketByIdAsync(id);
        if (existingTicket is null)
        {
            return NotFound();
        }
        await _ticketService.DeleteTicketAsync(id);
        return NoContent();
    }
}