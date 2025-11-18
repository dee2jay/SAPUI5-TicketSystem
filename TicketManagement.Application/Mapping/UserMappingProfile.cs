using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagementSystem.Application.Commands;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserCommand, User>().ConstructUsing(command => new User
        {
            Name = command.Nachname,
            Vorname = command.Vorname,
            Email = command.Email,
            Username = command.Username,
            Password = command.Password
        });
    }
}