using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Dtos;
public class UserDto
{ public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Passwort { get; set; } = string.Empty;
    public bool Useronnected { get; set; }
}