using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class CategoryRepository : ILibraryRepository<Category>
{
    private readonly LibraryContext _context;

    public CategoryRepository()
    {
    }
    public CategoryRepository(LibraryContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Categories.ToListAsync();
    }


    public virtual async Task<Category> GetByIdAsync(int id)
    {
        return (await _context.Categories.FindAsync(id))!;
    }

    public virtual async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public virtual async Task SaveAsync(Category category)
    {
        await _context.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id);        
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}