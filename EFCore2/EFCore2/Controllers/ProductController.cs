using EFCore2.Interfaces;
using EFCore2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFCore2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductStore<Product> _product;

    public ProductController(IProductStore<Product> product)
    {
        _product = product;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _product.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _product.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        //name category
        await _product.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.ProductId)
            return BadRequest();
        await _product.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _product.DeleteAsync(id);
        return NoContent();
    }
}