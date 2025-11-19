namespace TicketManagementSystem.Application.Command.UserCommands;

public record RegisterUserCommand(string Vorname, string Nachname, string Username, string Email, string Password);
