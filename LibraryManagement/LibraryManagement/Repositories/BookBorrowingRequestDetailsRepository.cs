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

    public async Task Add(BookBorrowingRequestDetails details)
    {
        _context.BookBorrowingRequestDetails.Add(details);
         await _context.SaveChangesAsync();
    }
    public async Task<BookBorrowingRequestDetails> GetRequestDetailByBookIdAndUserId(int bookId, int userId)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(r => r.BookBorrowingRequest)
            .Where(r => r.BookId == bookId && r.BookBorrowingRequest.UserId == userId && (r.BookBorrowingRequest.Status == "Pending" || r.BookBorrowingRequest.Status == "Approved"))
            .FirstOrDefaultAsync();
    }
}