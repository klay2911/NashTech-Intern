using LibraryManagement.Models;
using X.PagedList;

namespace LibraryManagement.Interfaces;

public interface IBookBorrowingRequestService
{
    Task<IPagedList<BookBorrowingRequest>> GetAllBorrowingRequests(int pageNumber, int pageSize);
    Task<BookBorrowingRequest> GetRequest(int requestId);
    Task<IPagedList<BookViewModel>> GetUserBorrowedBooks(int userId, int pageNumber, int pageSize, string searchTerm);
    Task CreateBorrowingRequestAsync(BookBorrowingRequest borrowingRequest, string bookIdsInRequestJson);
    Task<bool> CheckUserRequestLimit(int userId);
    Task UpdateRequestStatus(int requestId, string status, int librarianId);
}