using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Services;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAppLogger _logger;
    private readonly CancellationTokenSource _tokenSource = new();

    public UserController(IUserRepository userRepo, IAppLogger logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        try
        {
            _tokenSource.Token.ThrowIfCancellationRequested();
            var user = await _userService.RegisterUserAsync(dto, _tokenSource.Token);
            
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
            var token = await _userService.LoginUserAsync(dto, _tokenSource.Token);

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
        var user = await _userService.GetCurrentUser();
        await _userService.LogoutUserAsync(user.Email);
        
        return Ok(new
        {
            message = "User successfully logged out",
            email = user,
            connected = false
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.Identity?.Name;
        return Ok(new { userId, username });
    }
}