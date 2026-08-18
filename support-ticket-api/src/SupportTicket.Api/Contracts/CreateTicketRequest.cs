using System.ComponentModel.DataAnnotations;
using SupportTicket.Api.Models;

namespace SupportTicket.Api.Contracts;

public sealed class CreateTicketRequest
{
    [Required, StringLength(120, MinimumLength = 5)]
    public string Title { get; init; } = string.Empty;

    [Required, StringLength(2000, MinimumLength = 10)]
    public string Description { get; init; } = string.Empty;

    public TicketSeverity Severity { get; init; } = TicketSeverity.Medium;

    [StringLength(100)]
    public string? Assignee { get; init; }
}
