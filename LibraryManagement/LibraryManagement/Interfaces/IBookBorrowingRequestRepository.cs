using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestRepository
{
    IQueryable<BookBorrowingRequest> GetAll();
    Task<BookBorrowingRequest> GetByIdAsync(int id);
    Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest borrowingRequest);
    Task DeleteAsync(int id);
    Task<int> GetRequestsByUserThisMonth(int userId);
}