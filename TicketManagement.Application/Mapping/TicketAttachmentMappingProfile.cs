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
            UploadedAt = dto.UploadAt,
            Url = dto.Url,
            TicketId = dto.TicketId,
            UserId = dto.UserId
        });
    }
}