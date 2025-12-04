using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Mapping;

public class TicketCommentMappingProfile : Profile
{
    public TicketCommentMappingProfile()
    {
        CreateMap<TicketCommentDto, TicketComment>().ConstructUsing(dto => new TicketComment()
        {
            Author = dto.Author,
            Text = dto.Text,
        });
    }
}