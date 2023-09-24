using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestDetailsService
{
    IQueryable<BookBorrowingRequestDetails> GetAll();
    Task<BookBorrowingRequestDetails> GetBook(int requestId, int bookId);
    Task AddBookToRequest(BookBorrowingRequestDetails details);
    // Task RemoveBookFromRequest(int requestId, int bookId);
    Task<BookBorrowingRequestDetails> GetRequestDetail(int bookId);
}