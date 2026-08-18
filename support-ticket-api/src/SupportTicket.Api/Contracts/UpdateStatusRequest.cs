using SupportTicket.Api.Models;

namespace SupportTicket.Api.Contracts;

public sealed class UpdateStatusRequest
{
    public TicketStatus Status { get; init; }
}
