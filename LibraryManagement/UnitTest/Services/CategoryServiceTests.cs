using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class CategoryServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<CategoryRepository> _mockCategoryRepository;
    private CategoryService _categoryService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockCategoryRepository = new Mock<CategoryRepository>();
        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);
        _categoryService = new CategoryService(_mockUnitOfWork.Object);
    }
    /*[Test]
    public async Task GetAllCategoriesAsync_CategoriesExist_ReturnsAllCategories()
    {
        var categories = new List<Category> { new Category(), new Category() };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetAll()).ReturnsAsync(categories);
    
        var result = await _categoryService.GetAllCategoriesAsync();
    
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetCategoryByIdAsync_CategoryExists_ReturnsCategory()
    {
        var category = new Category { CategoryId = 1 };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetByIdAsync(1)).ReturnsAsync(category);

        var result = await _categoryService.GetCategoryByIdAsync(1);

        Assert.That(result, Is.EqualTo(category));
    }

    [Test]
    public async Task CreateCategoryAsync_NewCategory_ReturnsCreatedCategory()
    {
        var category = new Category();
        _mockUnitOfWork.Setup(u => u.CategoryRepository.CreateAsync(category)).ReturnsAsync(category);

        var result = await _categoryService.CreateCategoryAsync(category);

        Assert.That(result, Is.EqualTo(category));
    }

    [Test]
    public void UpdateCategoryAsync_CategoryDoesNotExist_ThrowsException()
    {
        var category = new Category { CategoryId = 1 };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetByIdAsync(1)).ReturnsAsync((Category)null);

        Assert.ThrowsAsync<Exception>(() => _categoryService.UpdateCategoryAsync(category));
    }

    [Test]
    public async Task DeleteCategoryAsync_CategoryExists_CallsDeleteOnRepository()
    {
        var category = new Category { CategoryId = 1 };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetByIdAsync(1)).ReturnsAsync(category);

        await _categoryService.DeleteCategoryAsync(1);

        _mockUnitOfWork.Verify(u => u.CategoryRepository.DeleteAsync(1), Times.Once);
    }*/
}
