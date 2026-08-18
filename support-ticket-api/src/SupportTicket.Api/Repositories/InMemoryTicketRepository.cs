using System.Collections.Concurrent;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Repositories;

public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly ConcurrentDictionary<Guid, SupportTicket> _tickets = new();

    public InMemoryTicketRepository()
    {
        var seed = new SupportTicket(
            Guid.NewGuid(),
            "Example login failure",
            "A seeded ticket used to demonstrate the list endpoint.",
            TicketSeverity.Medium,
            TicketStatus.Open,
            null,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        _tickets[seed.Id] = seed;
    }

    public IReadOnlyCollection<SupportTicket> GetAll() =>
        _tickets.Values.OrderByDescending(x => x.CreatedAt).ToArray();

    public SupportTicket? Get(Guid id) =>
        _tickets.TryGetValue(id, out var ticket) ? ticket : null;

    public SupportTicket Add(SupportTicket ticket)
    {
        if (!_tickets.TryAdd(ticket.Id, ticket))
            throw new InvalidOperationException("Ticket ID collision.");
        return ticket;
    }

    public bool Update(SupportTicket ticket)
    {
        if (!_tickets.TryGetValue(ticket.Id, out var current))
            return false;
        return _tickets.TryUpdate(ticket.Id, ticket, current);
    }

    public bool Delete(Guid id) => _tickets.TryRemove(id, out _);
}
