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

    public async Task<IEnumerable<Book>> GetAllAsync(bool includeCategory = false)
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

    // public virtual async Task<List<Book>> GetCurrentlyBorrowedBooks()
    // {
    //     return await _context.BookBorrowingRequestDetails
    //         .Include(d => d.Book)
    //         .Where(d => d.BookBorrowingRequest.Status == "Approved" && DateTime.Now <= d.BookBorrowingRequest.ExpiryDate)
    //         .Select(d => d.Book)
    //         .ToListAsync();
    // }
    //
    // public virtual async Task<List<Book>> GetBorrowedBooks(DateTime startTime, DateTime endTime)
    // {
    //     return await _context.BookBorrowingRequestDetails
    //         .Include(d => d.Book)
    //         .Where(d => d.BookBorrowingRequest.DateRequested >= startTime && d.BookBorrowingRequest.DateRequested <= endTime)
    //         .Select(d => d.Book)
    //         .ToListAsync();
    // }
    public virtual async Task<int> GetNumberOfBooks()
    {
        return await _context.Books.CountAsync();
    }
    public virtual async Task<List<Book>> GetTopReadBooksInMonth(int year, int month)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(d => d.Book)  
            .Where(d => d.BookBorrowingRequest.DateRequested.Year == year && d.BookBorrowingRequest.DateRequested.Month == month)
            .GroupBy(d => new { d.BookId, d.Book }) 
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.Book)
            .ToListAsync();
    }
}