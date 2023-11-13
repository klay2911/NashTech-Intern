using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

    public class BookBorrowingRequestRepository : IBookBorrowingRequestRepository
    {
        private readonly LibraryContext _context;

        public BookBorrowingRequestRepository()
        {
        }
        public BookBorrowingRequestRepository(LibraryContext context)
        {
            _context = context;
        }
        
        public virtual async Task<IEnumerable<BookBorrowingRequest>> GetAll()
        {
            return await _context.BorrowingRequests.Include(r => r.User).Where(r => r.Status == "Pending").ToListAsync();
        }

        public virtual async Task<BookBorrowingRequest> GetByIdAsync(int requestId)
        {
            return (await _context.BorrowingRequests
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.Book)
                .FirstOrDefaultAsync(r => r.RequestId == requestId))!;
        }

        public virtual async Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest borrowingRequest)
        {
            _context.BorrowingRequests.Add(borrowingRequest);
            await _context.SaveChangesAsync();
            return borrowingRequest;
        }
        public virtual void Update(BookBorrowingRequest entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task<int> GetRequestsByUserThisMonth(int userId)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _context.BorrowingRequests
                .Where(r => r.UserId == userId && r.DateRequested >= startOfMonth && r.DateRequested <= endOfMonth)
                .CountAsync();
        }

        public virtual async Task<List<BookBorrowingRequest>> GetRequestsByUser(int userId)
        {
            return await _context.BorrowingRequests
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.Book)
                .Where(r => r.UserId == userId )
                .ToListAsync();
                // && r.ExpiryDate > DateTime.Now
        }
        public virtual async Task UpdateRequestStatus(int requestId, string status, int librarianId)
        {
            var request = await _context.BorrowingRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = status;
                request.LibrarianId = librarianId;
                await _context.SaveChangesAsync();
            }
        }
        // public virtual async Task<List<BookBorrowingRequest>> GetCurrentBorrowingsForBook(int bookId)
        // {
        //     return await _context.BorrowingRequests
        //         .Include(b => b.BookBorrowingRequestDetails)
        //         .ThenInclude(d => d.Book)
        //         .Where(b => b.BookBorrowingRequestDetails.Any(d => d.BookId == bookId))
        //         .ToListAsync();
        // }
    }