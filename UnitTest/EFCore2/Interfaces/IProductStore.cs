
namespace EFCore2.Interfaces;

public interface IProductStore<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(int id);
    
}
