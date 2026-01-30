using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using NodaTime;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Results;
using TicketManagementSystem.Application.Security;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserLoginCommandHandler(
    IUserRepository userRepo,
    IRefreshTokenRepository refreshTokenRepo,
    IConfiguration config,
    IEventPublisher eventPublisher,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginUserCommand, LoginResult> {

    Task ICommandHandlerBase<LoginUserCommand>.Handle(LoginUserCommand command, CancellationToken ct)
    {
        return Handle(command, ct);
    }

    public async Task<LoginResult> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var userResult = await userRepo.GetUserByEmail(command.Email);
        if (userResult.IsError)
        {
            return LoginResult.Fail("USER_NOT_FOUND", "User not found");
        }
        var user = userResult.Value;

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
        {
            return LoginResult.Fail("INVALID_CREDENTIAL", 
                "Email or password " );
                                                         
        }

        // Device
        var deviceId = Guid.NewGuid().ToString();

        //Token
        var token = jwtProvider.Generate(user);
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

        return LoginResult.Ok(token, refreshToken, 
            SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromHours(refreshTokenExpirationInDays)));
    }
}