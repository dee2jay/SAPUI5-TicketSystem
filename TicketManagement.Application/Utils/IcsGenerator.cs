using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Utils
{
    public static class IcsGenerator
    {
        public static void CreateIcsFile(
            string filePath,
            string title,
            string description,
            DateTime start,
            DateTime end,
            string? location = null)
        {
            var sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//MyApp//EN");
            sb.AppendLine("METHOD:PUBLISH"); // Pas une invitation, juste un fichier calendrier
            sb.AppendLine("BEGIN:VEVENT");

            sb.AppendLine($"UID:{Guid.NewGuid()}");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTEND:{end:yyyyMMddTHHmmssZ}");

            sb.AppendLine($"SUMMARY:{Escape(title)}");
            sb.AppendLine($"DESCRIPTION:{Escape(description)}");

            if (!string.IsNullOrWhiteSpace(location))
                sb.AppendLine($"LOCATION:{Escape(location)}");

            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            return value
                .Replace(",", "\\,")
                .Replace(";", "\\;")
                .Replace("\n", "\\n");
        }
    }
}
