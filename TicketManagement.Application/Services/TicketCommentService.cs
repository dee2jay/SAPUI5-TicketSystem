using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.Services
{
    public class TicketCommentService(IMapper mapper, ITicketCommentRepository commentRepository, IEventPublisher eventPublisher, IAppLogger logger) : ITicketCommentService
    {
        public async Task<TicketComment> AddCommentsByTicketId(int ticketId, TicketCommentDto dto, CancellationToken ct)
        {
            try
            {
                var comment = mapper.Map<TicketComment>(dto);
                comment.TicketId = ticketId;
                await commentRepository.InsertComment(comment, ct);

                return comment;
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
                return null!;
            }
            
        }

    }
}
