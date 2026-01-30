using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Mapping;

public class TicketMappingProfile : Profile
{
    public TicketMappingProfile()
    {
        CreateMap<TicketDto, Ticket>().ConstructUsing(dto => new Ticket
        {
            Category = dto.Category,
            Location = dto.Location,
            CostCenter = dto.CostCenter,
            OrderNumber = dto.OrderNumber,
            Title = dto.Title,
            Description = dto.Description
        });
    }
}