using TicketManagementSystem.Application.Dtos;

namespace TicketManagementSystem.Application.Interfaces;

public interface ICategoryService : IAsyncDisposable
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);
    Task AddCategory(CategoryDto categoryDto, CancellationToken ct);
}