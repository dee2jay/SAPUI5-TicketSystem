using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);
    string GenerateRefreshToken();
}