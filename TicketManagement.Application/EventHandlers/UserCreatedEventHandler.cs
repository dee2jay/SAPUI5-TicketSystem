using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.EventHandlers;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent @event)
    {
        throw new NotImplementedException();
    }
}