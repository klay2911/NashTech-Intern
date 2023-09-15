using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class BorrowingRequestService
{
    private readonly BorrowingRequestRepository _borrowingRequestRepository;

    public BorrowingRequestService(BorrowingRequestRepository borrowingRequestRepository)
    {
        _borrowingRequestRepository = borrowingRequestRepository;
    }

    public async Task CreateBorrowingRequestAsync(BookBorrowingRequest request)
    {
        await _borrowingRequestRepository.CreateBookBorrowingRequestAsync(request);
    }

    public async Task<IEnumerable<BookBorrowingRequest>> GetBorrowingRequestsForUserAsync(int userId)
    {
        return await _borrowingRequestRepository.GetBookBorrowingRequestsForUserAsync(userId);
    }

    public async Task ApproveBorrowingRequestAsync(int requestId)
    {
        await _borrowingRequestRepository.ApproveBookBorrowingRequestAsync(requestId);
    }

    public async Task RejectBorrowingRequestAsync(int requestId)
    {
        await _borrowingRequestRepository.RejectBookBorrowingRequestAsync(requestId);
    }
}
