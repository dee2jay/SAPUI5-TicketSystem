using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Application.Dtos;
public class UserDto
{
    [Required]
    public string Vorname { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}