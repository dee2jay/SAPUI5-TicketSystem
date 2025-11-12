using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagement.Application.Mapping
{
    public class TicketMappingProfile : Profile
    {
        public TicketMappingProfile()
        {
            CreateMap<TicketDto, Ticket>().ConstructUsing(dto => new Ticket(
                dto.Category,
                dto.Location,
                dto.CostCenter,
                dto.OrderNumber,
                dto.Title,
                dto.Description));
        }
    }
}
