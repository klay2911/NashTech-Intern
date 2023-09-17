using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class BookBorrowingRequestService : IBookBorrowingRequestService
{
    private readonly UnitOfWork _unitOfWork;

    public BookBorrowingRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IQueryable<BookBorrowingRequest> GetAll()
    {
        return _unitOfWork.BookBorrowingRequestRepository.GetAll();
    }

    public async Task<BookBorrowingRequest> GetByIdAsync(int id)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetByIdAsync(id);
    }

    public async Task<BookBorrowingRequest> CreateAsync(BookBorrowingRequest borrowingRequest)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.CreateAsync(borrowingRequest);
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.BookBorrowingRequestRepository.DeleteAsync(id);
    }
}

