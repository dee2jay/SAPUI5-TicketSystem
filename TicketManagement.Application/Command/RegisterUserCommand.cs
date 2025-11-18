using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Commands;

public record RegisterUserCommand(string Vorname, string Nachname, string Username, string Email, string Password);
