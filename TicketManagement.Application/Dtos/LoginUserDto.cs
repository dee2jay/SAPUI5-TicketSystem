using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Application.Dtos;

public class LoginUserDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}