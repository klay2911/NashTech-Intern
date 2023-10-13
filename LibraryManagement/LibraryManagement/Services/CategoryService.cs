using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using X.PagedList;

namespace LibraryManagement.Services;

public class CategoryService : ICategoryService
{
    private readonly UnitOfWork _unitOfWork;

    public CategoryService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _unitOfWork.CategoryRepository.GetAll();
    }
    public async Task<IPagedList<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var categories = await _unitOfWork.CategoryRepository.GetAll();
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            categories = categories.Where(b => b.CategoryName.Contains(searchTerm));
        }
        
        int totalCount = categories.Count();
        
        var pagedCategories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        return new StaticPagedList<Category>(pagedCategories, pageNumber, pageSize, totalCount);
    }
    public async Task<IEnumerable<Category>> GetPagedCategoriesAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var categories = await _unitOfWork.CategoryRepository.GetAll();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            categories = categories.Where(c => c.CategoryName.Contains(searchTerm));
        }

        return categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }


    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _unitOfWork.CategoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        return await _unitOfWork.CategoryRepository.CreateAsync(category);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(category.CategoryId);
        if (existingCategory == null)
        {
            throw new Exception("The category with Id does not exist");
        }

        existingCategory.CategoryName = category.CategoryName;
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _unitOfWork.CategoryRepository.DeleteAsync(id);
    }
}