namespace SupportTicket.Api.Services;

public interface IAuditService
{
    void Record(string action, Guid ticketId, string actor);
}
