using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Services.AssignmentService;

public class TicketAssignmentService : ITicketAssignmentService
{
    private readonly IConfiguration _configuration;
    private readonly AssignmentRulesOptions _options;
    public TicketAssignmentService(IOptions<AssignmentRulesOptions> options, IConfiguration configuration)
    {
        _configuration = configuration;
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

