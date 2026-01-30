using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Domain.Models;

public class Category
{
    [Key] public int Id { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
}