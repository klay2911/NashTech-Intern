namespace LibraryManagement.Interfaces;

public interface ILibraryRepository<T> where T: class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(int id);
}