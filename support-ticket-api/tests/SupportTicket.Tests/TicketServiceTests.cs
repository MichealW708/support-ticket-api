using SupportTicket.Api.Contracts;
using SupportTicket.Api.Models;
using SupportTicket.Api.Repositories;
using SupportTicket.Api.Services;

namespace SupportTicket.Tests;

public sealed class TicketServiceTests
{
    private sealed class TestAuditService : IAuditService
    {
        public void Record(string action, Guid ticketId, string actor) { }
    }

    private static TicketService CreateService() =>
        new(new InMemoryTicketRepository(), new TestAuditService());

    [Fact]
    public void Create_StartsTicketAsOpen()
    {
        var service = CreateService();
        var request = new CreateTicketRequest
        {
            Title = "Customer cannot reset password",
            Description = "Reset email is not arriving after repeated attempts.",
            Severity = TicketSeverity.High
        };

        var ticket = service.Create(request, "test-agent");

        Assert.Equal(TicketStatus.Open, ticket.Status);
        Assert.Equal(TicketSeverity.High, ticket.Severity);
    }

    [Fact]
    public void UpdateStatus_ChangesExistingTicket()
    {
        var service = CreateService();
        var ticket = service.Create(new CreateTicketRequest
        {
            Title = "Application timeout issue",
            Description = "User reports recurring timeout while saving a record."
        }, "test-agent");

        var updated = service.UpdateStatus(ticket.Id, TicketStatus.Resolved, "test-agent");

        Assert.NotNull(updated);
        Assert.Equal(TicketStatus.Resolved, updated.Status);
    }

    [Fact]
    public void Delete_ReturnsFalseForUnknownTicket()
    {
        var service = CreateService();
        Assert.False(service.Delete(Guid.NewGuid(), "admin"));
    }
}
