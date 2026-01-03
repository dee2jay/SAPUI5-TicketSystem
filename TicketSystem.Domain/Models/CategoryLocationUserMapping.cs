using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Domain.Models;

public class CategoryLocationUserMapping
{
    [Key]
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Location  { get; set; }
    public string? MappedUser { get; set; }
}