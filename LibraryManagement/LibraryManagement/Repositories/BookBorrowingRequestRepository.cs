using LibraryManagement.Interfaces;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

    public class BookBorrowingRequestRepository : ILibraryRepository<BookBorrowingRequest>
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

        /*public void Delete(int id)
        {
            var request = GetById(id);
            if (request != null)
            {
                _context.BorrowingRequests.Remove(request);
                _context.SaveChanges();
            }
        }*/

    }