using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services;

public class BookBorrowingRequestService : IBookBorrowingRequestService
{
    private readonly UnitOfWork _unitOfWork;

    public BookBorrowingRequestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequests()
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetAll().ToListAsync();
    }

    // public async Task<BookBorrowingRequest> GetByIdAsync(int id)
    // {
    //     return await _unitOfWork.BookBorrowingRequestRepository.GetByIdAsync(id);
    // }
    public async Task<List<BookBorrowingRequest>> GetRequests(int userId)
    {
       return await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUser(userId);
    }

    public async Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest)
    { 
        await _unitOfWork.BookBorrowingRequestRepository.CreateAsync(borrowingRequest);
    }

    public async Task DeleteBorrowingRequestAsync(int id)
    {
        await _unitOfWork.BookBorrowingRequestRepository.DeleteAsync(id);
    }
    public async Task<int> GetNumberRequests(int userId)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUserThisMonth(userId);
    }

}

