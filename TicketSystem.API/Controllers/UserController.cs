using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using NodaTime;
using System.Security.Claims;
//using MediatR;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Security;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.API.Controllers;



[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, TicketDbContext dbContext) : ControllerBase
{
    private readonly CancellationTokenSource _tokenSource = new();

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        try
        {
            _tokenSource.Token.ThrowIfCancellationRequested();
            var user = await userService.RegisterUserAsync(dto, _tokenSource.Token);
            
            return Ok(new { user.Id, UserName = user.Username, user.Email });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost( "login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        try
        {
            _tokenSource.Token.ThrowIfCancellationRequested();
            var token = await userService.LoginUserAsync(dto, _tokenSource.Token);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var token))
            return Ok();

        var hash = JwtProvider.Hash(token);

        var rt = await dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == hash);
        if (rt != null)
        {
            rt.RevokedAt = SystemClock.Instance.GetCurrentInstant();
            await dbContext.SaveChangesAsync();
        }

        Response.Cookies.Delete("refreshToken");
        return Ok();
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll()
    {
        var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        var user = await dbContext.Users.FindAsync(userId);
        user!.TokenVersion++;

        var tokens = dbContext.RefreshTokens.Where(t => t.UserId == userId && t.RevokedAt == null);
        foreach (var t in tokens)
            t.RevokedAt = SystemClock.Instance.GetCurrentInstant();

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
       var user =await userService.GetCurrentUser();
       var fullname = $"{user.FirstName}, {user.LastName}";
       return Ok(new { user.Id, fullname });
    }
}