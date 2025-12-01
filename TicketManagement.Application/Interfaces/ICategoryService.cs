using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Dtos;

namespace TicketManagementSystem.Application.Interfaces;

public interface ICategoryService : IAsyncDisposable
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);
    Task AddCategory(CategoryDto categoryDto, CancellationToken ct);
}