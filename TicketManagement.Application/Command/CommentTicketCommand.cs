using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Command
{
    public record CommentTicketCommand(int TicketId, TicketComment Comment);

}
