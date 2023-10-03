using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestRepository
{
    Task<IEnumerable<BookBorrowingRequest>> GetAll();
    Task<BookBorrowingRequest> GetByIdAsync(int requestId);
    Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest borrowingRequest);
    Task<int> GetRequestsByUserThisMonth(int userId);
    Task<List<BookBorrowingRequest>> GetRequestsByUser(int userId);
    Task UpdateRequestStatus(int requestId, string status, int librarianId);
}