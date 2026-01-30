using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Services.Assignment;

public class TicketAssignmentService(IOptions<AssignmentRulesOptions> options, IConfiguration configuration)
    : ITicketAssignmentService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AssignmentRulesOptions _options = options.Value;

    public string? GetAssigneeForCategory(string category)
    {
        if (_options.Rules.TryGetValue(category, out var assignee))
        {
            return assignee;
        }
        return _options.Rules["default"]; // or return a default assignee
    }
}

