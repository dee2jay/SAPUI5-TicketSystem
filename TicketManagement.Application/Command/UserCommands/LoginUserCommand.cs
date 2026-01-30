using MediatR;
using TicketManagementSystem.Application.Results;

namespace TicketManagementSystem.Application.Command.UserCommands;

public record LoginUserCommand(string Email, string Password);