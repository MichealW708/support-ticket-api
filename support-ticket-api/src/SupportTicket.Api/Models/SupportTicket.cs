namespace SupportTicket.Api.Models;

public sealed record SupportTicket(
    Guid Id,
    string Title,
    string Description,
    TicketSeverity Severity,
    TicketStatus Status,
    string? Assignee,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
