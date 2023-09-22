using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

    public class BookBorrowingRequestRepository : IBookBorrowingRequestRepository
    {
        private readonly LibraryContext _context;

        public BookBorrowingRequestRepository(LibraryContext context)
        {
            _context = context;
        }

        public IQueryable<BookBorrowingRequest> GetAll()
        {
            return _context.BorrowingRequests;
        }

        public async Task<BookBorrowingRequest> GetByIdAsync(int id)
        {
            return (await _context.BorrowingRequests.FindAsync(id))!;
        }
        // return _context.BorrowingRequests.Include(b => b.BookBorrowingRequestDetails).FirstOrDefault(b => b.RequestId == id);


        public async Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest borrowingRequest)
        {
            _context.BorrowingRequests.Add(borrowingRequest);
            await _context.SaveChangesAsync();
            return borrowingRequest;
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _context.BorrowingRequests.FindAsync(id);
            if (request != null)
            {
                _context.BorrowingRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
        // public async Task<List<BookBorrowingRequest>> GetRequestsByUserAndMonth(int userId, int month)
        // {
        //     return await _context.BorrowingRequests
        //         .Where(r => r.UserId == userId && r.DateRequested.Month == month)
        //         .CountAsync();
        // }
        public async Task<int> GetRequestsByUserThisMonth(int userId)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _context.BorrowingRequests
                .Where(r => r.UserId == userId && r.DateRequested >= startOfMonth && r.DateRequested <= endOfMonth)
                .CountAsync();
        }


    }