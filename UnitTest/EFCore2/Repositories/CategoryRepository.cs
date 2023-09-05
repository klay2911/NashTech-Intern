using EFCore2.Interfaces;
using EFCore2.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore2.Repositories;

public class CategoryRepository : IProductStore<Category>
{
    private readonly StoreContext _context;

    public CategoryRepository()
    {
    }
    public CategoryRepository(StoreContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public virtual async Task<Category> GetByIdAsync(int id)
    {
        return (await _context.Categories.FindAsync(id))!;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task SaveAsync(Category category)
    {
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
