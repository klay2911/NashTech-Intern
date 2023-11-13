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
    public virtual async Task<IEnumerable<Book>> GetBooksBorrowedInPeriod(DateTime startDate, DateTime endDate)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(b => b.Book)
            .Where(b => b.BookBorrowingRequest.DateRequested >= startDate && b.BookBorrowingRequest.DateRequested <= endDate)
            .Select(b => b.Book)
            .Distinct()
            .ToListAsync();
    }
    public virtual async Task<IEnumerable<Book>> GetBooksNotBorrowedInPeriod(DateTime startDate, DateTime endDate)
    {
        var allBooks = await _context.Books.ToListAsync();

        var borrowedBooks = await _context.BookBorrowingRequestDetails
            .Include(b => b.Book)
            .Where(b => b.BookBorrowingRequest.DateRequested >= startDate && b.BookBorrowingRequest.DateRequested <= endDate)
            .Select(b => b.Book)
            .Distinct()
            .ToListAsync();

        var notBorrowedBooks = allBooks.Except(borrowedBooks);

        return notBorrowedBooks;
    }

    // public virtual async Task<IEnumerable<Book>> GetUserBooksBorrowedInPeriod(int userId, DateTime startDate, DateTime endDate)
    // {
    //     return await _context.BookBorrowingRequestDetails
    //         .Include(b => b.Book)
    //         .Where(b => b.BookBorrowingRequest.UserId == userId && b.BookBorrowingRequest.DateRequested >= startDate && b.BookBorrowingRequest.DateRequested <= endDate)
    //         .Select(b => b.Book)
    //         .Distinct()
    //         .ToListAsync();
    // }
    public virtual async Task<Dictionary<int, (string, int)>> GetNumberOfReadersPerBook(DateTime startDate, DateTime endDate)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(d => d.Book)
            .Where(d => d.BookBorrowingRequest.DateRequested >= startDate && d.BookBorrowingRequest.DateRequested <= endDate)
            .GroupBy(d => new { d.BookId, d.Book.Title }) 
            .Select(g => new { g.Key.BookId, g.Key.Title, NumberOfReaders = g.Select(d => d.BookBorrowingRequest.UserId).Distinct().Count() })
            .ToDictionaryAsync(x => x.BookId, x => (x.Title, x.NumberOfReaders));
    }
    public virtual async Task<List<User>> GetUsersBorrowingBook(int bookId)
    {
        return await _context.Users
            .Include(u => u.BookBorrowingRequests)
            .ThenInclude(r => r.BookBorrowingRequestDetails)
            .Where(u => u.BookBorrowingRequests.Any(r => r.BookBorrowingRequestDetails.Any(d => d.BookId == bookId)))
            .ToListAsync();
    }
}