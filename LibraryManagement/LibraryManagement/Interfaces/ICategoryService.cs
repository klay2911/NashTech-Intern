using LibraryManagement.Models;
using X.PagedList;

namespace LibraryManagement.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IPagedList<Category>>GetAllCategoriesAsync(int pageNumber, int pageSize, string searchTerm = "");
    Task<Category> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
}