using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementSystem.Application.Command;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler;

public class TicketCommentCommandHandler : ICommandHandlerBase<CommentTicketCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IEventPublisher _eventPublisher;

    public TicketCommentCommandHandler(ICommentRepository commentRepository, IEventPublisher eventPublisher)
    {
        _commentRepository = commentRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(CommentTicketCommand command)
    {

        await _eventPublisher.PublishEventAsync(null);
    }
}