namespace SupportTicket.Api.Services;

public sealed class AuditService(ILogger<AuditService> logger) : IAuditService
{
    public void Record(string action, Guid ticketId, string actor) =>
        logger.LogInformation(
            "AUDIT Action={Action} TicketId={TicketId} Actor={Actor}",
            action, ticketId, actor);
}
