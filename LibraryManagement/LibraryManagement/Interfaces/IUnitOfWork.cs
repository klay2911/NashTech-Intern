using LibraryManagement.Repositories;

namespace LibraryManagement.Interfaces;

public interface IUnitOfWork
{
    BookRepository BookRepository { get; }
    CategoryRepository CategoryRepository { get; }
    UserRepository UserRepository { get; }
    BookBorrowingRequestRepository BookBorrowingRequestRepository { get; }
    BookBorrowingRequestDetailsRepository BookBorrowingRequestDetailsRepository { get; }
    Task CreateTransaction();
    Task SaveAsync();
    Task CommitAsync();
    Task RollBackAsync();
}