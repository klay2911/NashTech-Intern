using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category entity);
    Task DeleteAsync(int id);
}