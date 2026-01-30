using Microsoft.Extensions.Options;
using TicketManagementSystem.Application.Services.Assignment;

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
        //_configuration.GetSection(SectionName).Bind(options);
        var section = _configuration.GetSection(SectionName);
        var dict = section.Get<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        // rebuild with case-insensitive comparer to allow lookups with different casing
        options.Rules = new Dictionary<string, string>(dict, StringComparer.OrdinalIgnoreCase);

    }
}