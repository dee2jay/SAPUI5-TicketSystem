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
        private readonly IAppLogger _logger;
        public UserRegisterCommandHandler(IUserRepository userRepo, IMapper mapper, IEventPublisher eventPublisher, IAppLogger logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<User> Handle(RegisterUserCommand command, CancellationToken ct)
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
            try
            {
                ct.ThrowIfCancellationRequested();
                await _userRepo.AddUser(user);
                await _eventPublisher.PublishEventAsync(userRegisterEvent,ct );
                return user;
            }
            catch (Exception e)
            {
                await _logger.LogError(e.Message, e, e.Source, e.StackTrace!);
                return null!;
            }
            
        }

        Task ICommandHandlerBase<RegisterUserCommand>.Handle(RegisterUserCommand command, CancellationToken ct)
        {
            return Handle(command, ct);
        }
    }
}
