using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Command.UserCommands;

public record LoginUserCommand(string Email, string Password);