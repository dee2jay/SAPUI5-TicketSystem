using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Services;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
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
        var user = await userService.GetCurrentUser();
        await userService.LogoutUserAsync(user.Email);
        
        return Ok(new
        {
            message = "User successfully logged out",
            email = user,
            connected = false
        });
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