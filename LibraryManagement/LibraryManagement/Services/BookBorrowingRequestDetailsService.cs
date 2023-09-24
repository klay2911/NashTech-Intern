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

    public IQueryable<BookBorrowingRequestDetails> GetAll()
    {
        return _unitOfWork.BookBorrowingRequestDetailsRepository.GetAll();
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

    // public async Task RemoveBookFromRequest(int requestId, int bookId)
    // {
    //     await _unitOfWork.BookBorrowingRequestDetailsRepository.Remove(requestId, bookId);
    // }

    public async Task<BookBorrowingRequestDetails> GetRequestDetail(int bookId)
    {
       return await _unitOfWork.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookId(bookId);
    }
}

