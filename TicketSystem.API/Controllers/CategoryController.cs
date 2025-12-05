using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService, IAppLogger logger) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [HttpGet("/api/categories",Name = "Categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        return Ok(await _categoryService.GetAllAsync(_cancellationTokenSource.Token));
    }

    [HttpPost("/api/addcategory", Name = "AddCategory")]
    public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] Category category)
    {
        try
        {
            await _categoryService.AddCategory(new CategoryDto
            {
                Key = category.Key,
                Value = category.Value
            }, _cancellationTokenSource.Token);

            return Ok(new CategoryDto
            {
                Key = category.Key,
                Value = category.Value
            });
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            return BadRequest();
        }
        
    }
}