using Microsoft.Extensions.Options;

namespace TicketManagementSystem.API.OptionsSetup;

public class SmtpSettingsOptionSetup : IConfigureOptions<SmtpSettingsOptionSetup>
{
    private const string SectionName = "SmtpSettings";
    private readonly IConfiguration _configuration;

    public SmtpSettingsOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SmtpSettingsOptionSetup options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}