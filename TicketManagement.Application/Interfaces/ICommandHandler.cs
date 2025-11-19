using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Interfaces;

public interface ICommandHandler<TCommand, TResult> : ICommandHandlerBase<TCommand>
{
    new Task<TResult> Handle(TCommand command);
}
