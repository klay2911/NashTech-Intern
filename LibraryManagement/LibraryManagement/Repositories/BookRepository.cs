using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BookRepository : ILibraryRepository<Book>
{
    private readonly LibraryContext _context;

    public BookRepository()
    {
    }
    public BookRepository(LibraryContext context)
    {
        _context = context;
    }
  
    public virtual async Task<IEnumerable<Book>> GetAll()
    {
        return await _context.Books.ToListAsync();
    }

    public virtual async Task<IEnumerable<Book>> GetAllAsync(bool includeCategory = false)
    {
        if (includeCategory)
        {
           return _context.Books.Include(b => b.Category).OrderBy(b=>b.Title);
        }

        return await _context.Books.ToListAsync();

    }

    public virtual async Task<Book> GetByIdAsync(int id)
    {
        return (await _context.Books.FindAsync(id))!;
    }

    public virtual async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public virtual async Task SaveAsync(Book book)
    {
        await _context.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
// public async Task<List<ICollection<Book>>> GetBooksForUserAsync(int userId)
// {
//     return await _context.BookBorrowingRequestDetails
//         .Where(b => b.RequestId == userId && b.BookBorrowingRequestRepository.Status == "Approved")
//         .Select(b => b.Books)
//         .ToListAsync();
// }