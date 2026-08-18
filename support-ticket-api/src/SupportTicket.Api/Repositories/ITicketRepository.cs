using SupportTicket.Api.Models;

namespace SupportTicket.Api.Repositories;

public interface ITicketRepository
{
    IReadOnlyCollection<SupportTicket> GetAll();
    SupportTicket? Get(Guid id);
    SupportTicket Add(SupportTicket ticket);
    bool Update(SupportTicket ticket);
    bool Delete(Guid id);
}
