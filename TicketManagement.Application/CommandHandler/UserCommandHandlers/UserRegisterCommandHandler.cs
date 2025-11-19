using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers
{
    public class UserRegisterCommandHandler : ICommandHandler<RegisterUserCommand, User>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        public UserRegisterCommandHandler(IUserRepository userRepo, IMapper mapper, IEventPublisher eventPublisher)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<User> Handle(RegisterUserCommand command)
        {
            var user = new User
            {
                Vorname = command.Vorname,
                Name = command.Nachname,
                Username = command.Username,
                Email = command.Email,
                Password = string.Empty,
                UserConnected = false
            };
            
            user.Password = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var userRegisterEvent = new UserCreatedEvent(
                command.Vorname,
                command.Nachname,
                command.Email,
                command.Username,
                DateTime.Now);

            await _userRepo.AddUser(user);
            await _eventPublisher.PublishEventAsync(userRegisterEvent);
            return user;
        }

        Task ICommandHandlerBase<RegisterUserCommand>.Handle(RegisterUserCommand command)
        {
            return Handle(command);
        }
    }
}
