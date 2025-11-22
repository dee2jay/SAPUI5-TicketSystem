using Microsoft.Extensions.Options;
using TicketManagementSystem.Application.Services.AssignmentService;

namespace TicketManagementSystem.API.OptionsSetup;

public class AssignmentRulesOptionsSetup : IConfigureOptions<AssignmentRulesOptions>
{
    private const string SectionName = "AssignmentRules";
    private readonly IConfiguration _configuration;

    public AssignmentRulesOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AssignmentRulesOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}