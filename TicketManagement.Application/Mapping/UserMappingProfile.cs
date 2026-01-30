using AutoMapper;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserCommand, User>().ConstructUsing(command => new User
        {
            LastName = command.Lastname,
            FirstName = command.Firstname,
            Email = command.Email,
            Username = command.Username,
            Password = command.Password
        });
    }
}