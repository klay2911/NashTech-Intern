using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BookBorrowingRequestDetailsRepository : ILibraryRepository<BookBorrowingRequestDetails>
{
    private readonly LibraryContext _context;
    public BookBorrowingRequestDetailsRepository()
    {
    }
    public BookBorrowingRequestDetailsRepository(LibraryContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<BookBorrowingRequestDetails>> GetAll()
    {
        return await _context.BookBorrowingRequestDetails.ToListAsync();
    }

    public Task<BookBorrowingRequestDetails> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BookBorrowingRequestDetails> CreateAsync(BookBorrowingRequestDetails entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<BookBorrowingRequestDetails> GetBookByRequestId(int requestId, int bookId)
    {
        return (await _context.BookBorrowingRequestDetails.FirstOrDefaultAsync(b => b.RequestId == requestId && b.BookId == bookId))!;
    }

    public virtual async Task Add(BookBorrowingRequestDetails details)
    {
        _context.BookBorrowingRequestDetails.Add(details);
         await _context.SaveChangesAsync();
    }
    public virtual async Task<BookBorrowingRequestDetails> GetRequestDetailByBookIdAndUserId(int bookId, int userId)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(r => r.BookBorrowingRequest)
            .Where(r => r.BookId == bookId && r.BookBorrowingRequest.UserId == userId && (r.BookBorrowingRequest.Status == "Pending" || r.BookBorrowingRequest.Status == "Approved"))
            .FirstOrDefaultAsync();
    }
}