using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestDetailsRepository
{
    IQueryable<BookBorrowingRequestDetails> GetAll();
    Task<BookBorrowingRequestDetails> GetBookByRequestId(int requestId, int bookId);
    Task RemoveBookFromRequest(int requestId, int bookId);
}