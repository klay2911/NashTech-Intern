using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly LibraryContext _context;

    public CategoryRepository()
    {
    }
    public CategoryRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
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