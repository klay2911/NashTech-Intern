using EFCore2.Interfaces;
using EFCore2.Models;
using EFCore2.Repositories;

namespace EFCore2;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly StoreContext _context;
    public UnitOfWork()
    {
        
    }
    public UnitOfWork(StoreContext context)
    {
        _context = context;
        ProductRepository = new ProductRepository(context);
        CategoryRepository = new CategoryRepository(context);
    }

    public virtual ProductRepository ProductRepository { get; }

    public virtual CategoryRepository CategoryRepository { get; }

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
