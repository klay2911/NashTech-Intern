using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestDetailsService
{
    Task<IEnumerable<BookBorrowingRequestDetails>> GetAll();
    Task<BookBorrowingRequestDetails> GetBook(int requestId, int bookId);
    Task AddBookToRequest(BookBorrowingRequestDetails details);
    Task<Book> CheckExistingRequest(int userId, List<int> bookIds);
    Task SaveLastReadPageNumber(int bookId, int userId, int pageNumber);
    Task<IEnumerable<Book>> GetBooksBorrowedInPeriod(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Book>> GetBooksNotBorrowedInPeriod(DateTime startDate, DateTime endDate);
    Task<Dictionary<int, (string, int)>> GetNumberOfReadersPerBook(DateTime startDate, DateTime endDate);
    Task<List<User>> GetUsersBorrowingBook(int bookId);
}