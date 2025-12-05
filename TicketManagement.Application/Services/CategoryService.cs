using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.Services
{
    public class CategoryService(TicketDbContext db, IAppLogger logger) : ICategoryService
    {
        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                return await db.Categories
                    .Select(x => new CategoryDto
                    {
                        Key = x.Key,
                        Value = x.Value
                    })
                    .ToListAsync(cancellationToken: ct);
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
                return [];
            }
            
        }

        public async Task AddCategory(CategoryDto categoryDto, CancellationToken ct)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                var newCategory = new Category { Key = categoryDto.Key, Value = categoryDto.Value };
                db.Categories.Add(newCategory);
                await db.SaveChangesAsync(ct);
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            }
            
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return ValueTask.CompletedTask;
        }
    }
}
