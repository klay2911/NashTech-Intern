using EFCore2.Interfaces;
using EFCore2.Models;

namespace EFCore2.Services;

public class CategoryService : ICategoryService
{
    private readonly UnitOfWork _unitOfWork;

    public CategoryService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _unitOfWork.CategoryRepository.GetAllAsync();
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
            throw new Exception("The category with Id does not exsist");
        }

        existingCategory.CategoryName = category.CategoryName;
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _unitOfWork.CategoryRepository.DeleteAsync(id);
    }
}