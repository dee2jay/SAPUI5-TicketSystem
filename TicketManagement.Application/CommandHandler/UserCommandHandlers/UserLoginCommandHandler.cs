using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Publisher;
using TicketManagementSystem.Application.Security;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers
{

    public class UserLoginCommandHandler : ICommandHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEventPublisher _eventPublisher;
        private readonly IJwtProvider _jwtProvider;
        public UserLoginCommandHandler(IUserRepository userRepo, IMapper mapper, IConfiguration config, IEventPublisher eventPublisher, IJwtProvider jwtProvider)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
            _eventPublisher = eventPublisher;
            _jwtProvider = jwtProvider;
        }

        Task ICommandHandlerBase<LoginUserCommand>.Handle(LoginUserCommand command, CancellationToken ct)
        {
            return Handle(command, ct);
        }

        public async Task<string> Handle(LoginUserCommand command, CancellationToken ct)
        {
            var user = await _userRepo.GetUserByEmail(command.Email);
            if (!user.IsError)
            {
                if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Value.Password))
                    return null!;
            }
            user.Value.UserConnected = true;

            var userLoginEvent = new UserLoginEvent(user.Value.Email, user.Value.Username);
            await _userRepo.UpdateUser(user.Value);
            await _eventPublisher.PublishEventAsync(userLoginEvent, CancellationToken.None);

            return _jwtProvider.Generate(user.Value);
        }
    }
}
