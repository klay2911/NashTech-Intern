using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BorrowingRequestRepository
{
    private readonly LibraryContext _context;

    public BorrowingRequestRepository()
    {
    }
    public BorrowingRequestRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task CreateBookBorrowingRequestAsync(BookBorrowingRequest request)
    {
        _context.BorrowingRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookBorrowingRequest>> GetBookBorrowingRequestsForUserAsync(int userId)
    {
        return await _context.BorrowingRequests
            .Include(r => r.BookBorrowingRequestDetails)
            .ThenInclude(d => d.Books)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task ApproveBookBorrowingRequestAsync(int requestId)
    {
        var request = await _context.BorrowingRequests.FindAsync(requestId);
        if (request != null)
        {
            request.Status = "Approved";
            await _context.SaveChangesAsync();
        }
    }

    public async Task RejectBookBorrowingRequestAsync(int requestId)
    {
        var request = await _context.BorrowingRequests.FindAsync(requestId);
        if (request != null)
        {
            request.Status = "Rejected";
            await _context.SaveChangesAsync();
        }
    }
}
