namespace TicketManagementSystem.Application.Command.UserCommands;

public record RegisterUserCommand(string Firstname, string Lastname, string Username, string Email, string Password);
