using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Mapping;

public class TicketAttachmentMappingProfile : Profile
{
    public TicketAttachmentMappingProfile()
    {
        CreateMap<TicketAttachmentDto, TicketAttachment>().ConstructUsing(dto => new TicketAttachment
        {
            FileName = dto.FileName,
            Data = dto.Data,
            Url = dto.Url,
        });
    }
}