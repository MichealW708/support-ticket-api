using SupportTicket.Api.Contracts;
using SupportTicket.Api.Models;
using SupportTicket.Api.Repositories;

namespace SupportTicket.Api.Services;

public sealed class TicketService(ITicketRepository repository, IAuditService audit) : ITicketService
{
    public IReadOnlyCollection<SupportTicket> GetAll() => repository.GetAll();

    public SupportTicket? Get(Guid id) => repository.Get(id);

    public SupportTicket Create(CreateTicketRequest request, string actor)
    {
        var now = DateTimeOffset.UtcNow;
        var ticket = new SupportTicket(
            Guid.NewGuid(),
            request.Title.Trim(),
            request.Description.Trim(),
            request.Severity,
            TicketStatus.Open,
            string.IsNullOrWhiteSpace(request.Assignee) ? null : request.Assignee.Trim(),
            now,
            now);

        repository.Add(ticket);
        audit.Record("TicketCreated", ticket.Id, actor);
        return ticket;
    }

    public SupportTicket? UpdateStatus(Guid id, TicketStatus status, string actor)
    {
        var current = repository.Get(id);
        if (current is null) return null;

        var updated = current with { Status = status, UpdatedAt = DateTimeOffset.UtcNow };
        if (!repository.Update(updated)) return null;

        audit.Record("TicketStatusChanged", id, actor);
        return updated;
    }

    public bool Delete(Guid id, string actor)
    {
        var deleted = repository.Delete(id);
        if (deleted) audit.Record("TicketDeleted", id, actor);
        return deleted;
    }
}
