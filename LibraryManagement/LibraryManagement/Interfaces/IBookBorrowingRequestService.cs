using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestService
{
    IQueryable<BookBorrowingRequest> GetAll();
    Task<BookBorrowingRequest> GetByIdAsync(int id);
    Task CreateAsync(BookBorrowingRequest borrowingRequest);
    Task DeleteAsync(int id);
    Task<int> GetRequestsByUserThisMonth(int userId);
}