using System.Security.Authentication;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NodaTime;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Security;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserLoginCommandHandler(
    IUserRepository userRepo,
    IRefreshTokenRepository refreshTokenRepo,
    IMapper mapper,
    IConfiguration config,
    IEventPublisher eventPublisher,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginUserCommand, string> {

    Task ICommandHandlerBase<LoginUserCommand>.Handle(LoginUserCommand command, CancellationToken ct)
    {
        return Handle(command, ct);
    }

    public async Task<string> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var userResult = await userRepo.GetUserByEmail(command.Email);
        if (userResult.IsError)
        {
            throw new UnauthorizedAccessException();
        }
        var user = userResult.Value;

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
        {
            throw new UnauthorizedAccessException();
        }

        // Device
        var deviceId = Guid.NewGuid().ToString();

        // Refresh token
        var refreshToken = jwtProvider.GenerateRefreshToken();
        var refreshTokenHash = JwtProvider.Hash(refreshToken);

        var refreshTokenExpirationInDaysString = config.GetSection("Jwt")["RefreshTokenExpirationInDays"];
        if (!int.TryParse(refreshTokenExpirationInDaysString, out var refreshTokenExpirationInDays))
        {
            throw new InvalidOperationException("Invalid RefreshTokenExpirationInDays configuration value.");
        }
        var refreshTokenEntity = new Domain.Models.RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = SystemClock.Instance.GetCurrentInstant() + Duration.FromDays(refreshTokenExpirationInDays),
            DeviceId = deviceId,
        };
        await refreshTokenRepo.AddRefreshToken(refreshTokenEntity);

        var userLoginEvent = new UserLoginEvent(user.Email, user.Username, deviceId);
        await userRepo.UpdateUser(user);
        await eventPublisher.PublishEventAsync(userLoginEvent, ct);

        return jwtProvider.Generate(user);
    }
}