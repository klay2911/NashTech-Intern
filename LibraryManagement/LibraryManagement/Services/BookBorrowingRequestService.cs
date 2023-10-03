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

    public async Task<IEnumerable<BookBorrowingRequest>> GetAllBorrowingRequests()
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetAll();
    }

    public async Task<BookBorrowingRequest> GetRequest(int requestId)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetByIdAsync(requestId);
    }
    public async Task<List<BookBorrowingRequest>> GetUserRequests(int userId)
    {
       return await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUser(userId);
    }
    public async Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest)
    { 
        await _unitOfWork.BookBorrowingRequestRepository.CreateAsync(borrowingRequest);
    }
    public async Task<int> GetNumberRequests(int userId)
    {
        return await _unitOfWork.BookBorrowingRequestRepository.GetRequestsByUserThisMonth(userId);
    }
    public async Task UpdateRequestStatus(int requestId, string status, int librarianId)
    {
        await _unitOfWork.BookBorrowingRequestRepository.UpdateRequestStatus(requestId, status, librarianId);
    }
}

