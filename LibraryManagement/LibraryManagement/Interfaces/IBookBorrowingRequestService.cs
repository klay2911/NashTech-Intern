using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestService
{
    Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequests();
    Task<BookBorrowingRequest> GetRequest(int requestId);
    Task<List<BookBorrowingRequest>> GetUserRequests(int userId);
    Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest);
    Task<int> GetNumberRequests(int userId);
    Task UpdateRequestStatus(int requestId, string status, int librarianId);
}