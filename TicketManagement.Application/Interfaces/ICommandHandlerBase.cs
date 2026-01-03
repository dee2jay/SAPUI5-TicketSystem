namespace TicketManagementSystem.Application.Interfaces;

public interface ICommandHandlerBase <TCommand>
{
    Task Handle(TCommand command, CancellationToken ct);
}