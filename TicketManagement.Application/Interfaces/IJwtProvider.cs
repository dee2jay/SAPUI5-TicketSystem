using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);

}