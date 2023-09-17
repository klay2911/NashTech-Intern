using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BookBorrowingRequestDetailsRepository : IBookBorrowingRequestDetailsRepository
{
    private readonly LibraryContext _context;

    public BookBorrowingRequestDetailsRepository(LibraryContext context)
    {
        _context = context;
    }

    public IQueryable<BookBorrowingRequestDetails> GetAll()
    {
        return _context.BookBorrowingRequestDetails;
    }

    public async Task<BookBorrowingRequestDetails> GetBookByRequestId(int requestId, int bookId)
    {
        return (await _context.BookBorrowingRequestDetails.FirstOrDefaultAsync(b => b.RequestId == requestId && b.BookId == bookId))!;
    }

    public async Task AddBookToRequest(BookBorrowingRequestDetails details)
    {
        _context.BookBorrowingRequestDetails.Add(details);
         await _context.SaveChangesAsync();
    }
    

    public async Task RemoveBookFromRequest(int requestId, int bookId)
    {
        var details = GetBookByRequestId(requestId, bookId);
        if (details != null)
        {
            _context.BookBorrowingRequestDetails.Remove(await details);
           await _context.SaveChangesAsync();
        }
    }
}