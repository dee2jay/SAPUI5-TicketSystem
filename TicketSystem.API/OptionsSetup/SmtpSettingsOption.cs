using Microsoft.Extensions.Options;

namespace TicketManagementSystem.API.OptionsSetup
{
    public class SmtpSettingsOption : IConfigureOptions<SmtpSettingsOption>
    {
        private const string SectionName = "SmtpSettings";
        private readonly IConfiguration _configuration;

        public SmtpSettingsOption(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(SmtpSettingsOption options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
