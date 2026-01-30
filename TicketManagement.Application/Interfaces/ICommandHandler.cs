using TicketManagementSystem.Application.Results;

namespace TicketManagementSystem.Application.Interfaces;

public interface ICommandHandler<TCommand, TResult> : ICommandHandlerBase<TCommand>
{
    new Task<TResult> Handle(TCommand command, CancellationToken ct);
}
