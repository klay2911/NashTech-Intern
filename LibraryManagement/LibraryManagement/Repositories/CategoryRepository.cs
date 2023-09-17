using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public sealed class CategoryRepository : ILibraryRepository<Category>
{
    private readonly LibraryContext _context;

    public CategoryRepository()
    {
    }
    public CategoryRepository(LibraryContext context)
    {
        _context = context;
    }

    public  IQueryable<Category> GetAll()
    {
        return _context.Categories;
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
        var category = await _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id);        
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}