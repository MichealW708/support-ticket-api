using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportTicket.Api.Contracts;
using SupportTicket.Api.Models;
using SupportTicket.Api.Services;

namespace SupportTicket.Api.Controllers;

[ApiController]
[Authorize(Roles = "Agent,Admin")]
[Route("api/[controller]")]
public sealed class TicketsController(ITicketService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<SupportTicket>> GetAll() => Ok(service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<SupportTicket> Get(Guid id)
    {
        var ticket = service.Get(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public ActionResult<SupportTicket> Create(CreateTicketRequest request)
    {
        var created = service.Create(request, User.Identity?.Name ?? "unknown");
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    public ActionResult<SupportTicket> UpdateStatus(Guid id, UpdateStatusRequest request)
    {
        var updated = service.UpdateStatus(id, request.Status, User.Identity?.Name ?? "unknown");
        return updated is null ? NotFound() : Ok(updated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) =>
        service.Delete(id, User.Identity?.Name ?? "unknown") ? NoContent() : NotFound();
}
