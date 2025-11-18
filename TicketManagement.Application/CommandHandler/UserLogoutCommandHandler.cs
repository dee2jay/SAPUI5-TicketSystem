using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Command;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Publisher;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler
{
    public class UserLogoutCommandHandler : ICommandHandlerBase<LogoutUserCommand>
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventPublisher _eventPublisher;

        public UserLogoutCommandHandler(IUserRepository userRepo, IEventPublisher eventPublisher)
        {
            _userRepo = userRepo;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(LogoutUserCommand cmd)
        {
            var user = await _userRepo.GetUserByEmail(cmd.Email);
            if (!user.IsError)
            {
                user.Value.UserConnected = false;

                await _userRepo.UpdateUser(user.Value);
                await _eventPublisher.PublishEventAsync(new UserLogoutEvent(user.Value.Email, user.Value.Username, DateTime.UtcNow));
            }
        }
    }
}
