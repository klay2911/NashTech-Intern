using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class BookBorrowingRequestDetailsService : IBookBorrowingRequestDetailsService
{
    private readonly UnitOfWork _unitOfWork;
    public BookBorrowingRequestDetailsService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BookBorrowingRequestDetails>> GetAll()
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetAll();
    }

    public async Task<BookBorrowingRequestDetails> GetBook(int requestId, int bookId)
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetBookByRequestId(requestId, bookId);
    }

    public async Task AddBookToRequest(BookBorrowingRequestDetails details)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(details.BookId);
        if (book == null)
        {
            throw new Exception("The book no longer exists, please refresh the page.");
        }

        await _unitOfWork.BookBorrowingRequestDetailsRepository.Add(details);
    }

    public async Task SaveLastReadPageNumber(int bookId, int userId, int pageNumber)
    {
        var detail = await _unitOfWork.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookIdAndUserId(bookId, userId);

        if (detail != null)
        {
            detail.LastReadPageNumber = pageNumber;
            // await _unitOfWork.CommitAsync();
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task<Book> CheckExistingRequest(int userId, List<int> bookIds)
    {
        foreach (var bookId in bookIds)
        {
            var existingRequestDetail = await _unitOfWork.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookIdAndUserId(bookId, userId);
            if (existingRequestDetail != null)
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book != null)
                {
                    return book;
                }
            }
        }
        return null;
    }

    public async Task<IEnumerable<Book>> GetBooksBorrowedInPeriod(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetBooksBorrowedInPeriod(startDate, endDate);
    }

    public async Task<IEnumerable<Book>> GetBooksNotBorrowedInPeriod(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetBooksNotBorrowedInPeriod(startDate, endDate);
    }
    public async Task<Dictionary<int, (string, int)>> GetNumberOfReadersPerBook(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetNumberOfReadersPerBook(startDate, endDate);
    }

    public async Task<List<User>> GetUsersBorrowingBook(int bookId)
    {
        return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetUsersBorrowingBook(bookId);
    }
}

