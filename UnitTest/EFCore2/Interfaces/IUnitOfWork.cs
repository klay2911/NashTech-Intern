using EFCore2.Repositories;

namespace EFCore2.Interfaces;

public interface IUnitOfWork
{
    ProductRepository ProductRepository { get; }
    CategoryRepository CategoryRepository { get; }
    Task CreateTransaction();
    Task SaveAsync();
    Task CommitAsync();
    Task RollBackAsync();

}
