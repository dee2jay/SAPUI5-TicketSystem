using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        return Ok(await _categoryService.GetAllAsync(_cancellationTokenSource.Token));
    }

    [HttpPost("/api/addcategory", Name = "AddCategory")]
    public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] string category)
    {
        await _categoryService.AddCategory(new CategoryDto
        {
            Name = category
        }, _cancellationTokenSource.Token);

        return new CategoryDto() { Name = category };
    }
}