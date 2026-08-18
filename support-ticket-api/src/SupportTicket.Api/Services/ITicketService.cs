using SupportTicket.Api.Contracts;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Services;

public interface ITicketService
{
    IReadOnlyCollection<SupportTicket> GetAll();
    SupportTicket? Get(Guid id);
    SupportTicket Create(CreateTicketRequest request, string actor);
    SupportTicket? UpdateStatus(Guid id, TicketStatus status, string actor);
    bool Delete(Guid id, string actor);
}
