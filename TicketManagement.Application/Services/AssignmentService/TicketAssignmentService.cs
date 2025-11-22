using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Services.AssignmentService;

public class TicketAssignmentService : ITicketAssignmentService
{
    private readonly AssignmentRulesOptions _options;
    public TicketAssignmentService(IOptions<AssignmentRulesOptions> options)
    {
        _options = options.Value;
    }
    public string? GetAssigneeForCategory(string category)
    {
        if (_options.Rules.TryGetValue(category, out var assignee))
        {
            return assignee;
        }
        return _options.Rules["default"]; // or return a default assignee
    }
}

