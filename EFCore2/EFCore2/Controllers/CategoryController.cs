using EFCore2.Interfaces;
using EFCore2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFCore2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IProductStore<Category> _category;
    public CategoryController( IProductStore<Category> category)
    {
        _category = category;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _category.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _category.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        await _category.CreateAsync(category);
        return CreatedAtAction("GetById", new { id = category.CategoryId }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.CategoryId)
            return BadRequest();
        await _category.UpdateAsync(category);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _category.DeleteAsync(id);
        return NoContent();
    }
}