using EFCore2.Interfaces;
using EFCore2.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore2.Repositories;


public class ProductRepository: IProductStore<Product>
{
    private readonly StoreContext _context;

    public ProductRepository()
    {
    }
    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public virtual async Task<Product> GetByIdAsync(int id)
    {
        return (await _context.Products.FindAsync(id))!;
    }

    public virtual async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public virtual async Task SaveChanges(Product product)
    {
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}