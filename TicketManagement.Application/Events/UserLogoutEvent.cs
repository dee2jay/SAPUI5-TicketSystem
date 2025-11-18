using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events;

public record UserLogoutEvent(string Email, string Username, DateTime Timestamp) : IDomainEvent;