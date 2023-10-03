using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestDetailsService
{
    Task<IEnumerable<BookBorrowingRequestDetails>> GetAll();
    Task<BookBorrowingRequestDetails> GetBook(int requestId, int bookId);
    Task AddBookToRequest(BookBorrowingRequestDetails details);
    Task<BookBorrowingRequestDetails> GetRequestDetail(int bookId, int userId);
}