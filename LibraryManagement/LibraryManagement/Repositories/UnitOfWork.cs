using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly LibraryContext _context;
    public UnitOfWork()
    {
        
    }
    public UnitOfWork(LibraryContext context)
    {
        _context = context;
        BookRepository = new BookRepository(context);
        CategoryRepository = new CategoryRepository(context);
        UserRepository = new UserRepository(context);
        // BorrowingRequestRepository = new BorrowingRequestRepository(context);
        // BorrowingRequestDetailsRepository = new BorrowingRequestDetailsRepository();
    }
    public virtual BookRepository BookRepository { get; }
    public virtual CategoryRepository CategoryRepository { get; }
    public virtual UserRepository UserRepository { get; }
    // public virtual BorrowingRequestRepository BorrowingRequestRepository{ get; }
    // public virtual BorrowingRequestDetailsRepository BorrowingRequestDetailsRepository { get; }


    public virtual async Task CreateTransaction()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        if (transaction == null)
        {
            throw new Exception("Failed to create a transaction.");
        }
    }
    public virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    public virtual async Task CommitAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public virtual async Task RollBackAsync()
    {
        await _context.Database.RollbackTransactionAsync();
        await _context.DisposeAsync();
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}