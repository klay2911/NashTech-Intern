using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository()
    {
    }

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }
    public  IQueryable<Book> GetAll()
    {
        return _context.Books;
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        return (await _context.Books.FindAsync(id))!;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task SaveAsync(Book book)
    {
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<ICollection<Book>>> GetBooksForUserAsync(int userId)
    {
        return await _context.BookBorrowingRequestDetails
            .Where(b => b.RequestId == userId && b.BookBorrowingRequest.Status == "Approved")
            .Select(b => b.Books)
            .ToListAsync();
    }
}