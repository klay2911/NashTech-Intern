using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestService
{
    Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequests();
    Task<List<BookBorrowingRequest>> GetRequests(int userId);
    Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest);
    Task DeleteBorrowingRequestAsync(int id);
    Task<int> GetNumberRequests(int userId);
}