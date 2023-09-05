using EFCore2;
using EFCore2.Models;
using EFCore2.Repositories;
using EFCore2.Services;
using Moq;

namespace UnitTest.Services;

public class ProductServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<ProductRepository> _mockProductRepository;
    //private Mock<CategoryRepository> _mockCategoryRepository;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockProductRepository = new Mock<ProductRepository>();
        //_mockCategoryRepository = new Mock<CategoryRepository>();
        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
        _productService = new ProductService(_mockUnitOfWork.Object);

    }

    [Test]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Product 1", Manufacture = "Viet Nam", CategoryId = 1 },
            new Product { ProductId = 2, ProductName = "Product 2", Manufacture = "Viet Nam", CategoryId = 1 }
        };
        _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.That(result, Is.EqualTo(products));
    }

    [Test]
    public async Task GetProductById_ReturnsProductWithGivenId()
    {
        // Arrange
        var product = new Product
            { ProductId = 1, ProductName = "Product 1", Manufacture = "Viet Nam", CategoryId = 1 };
        _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        // compare from count to
        Assert.That(result, Is.EqualTo(product));
    }
    //Test data
    //verify object
    // rollback mirgration
    // how to know mirgration run
    [Test]
    public async Task CreateProductAsync_ValidProduct_ReturnsSuccess()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            Manufacture = "Test Manufacture",
            CategoryId = 1
        };

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _productService.CreateProductAsync(product);

        // Assert
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Once);
    }
    [Test]
    public async Task CreateProductAsync_NullCategory_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            CategoryId = 5,
            Manufacture = "Test Manufacture"
        };
        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(null as Category);

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(() => _productService.CreateProductAsync(product));
        Assert.That(exception.Message, Is.EqualTo($"The CategoryId {product.CategoryId} does not exist."));
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Never);
    }
    [Test]
    public async Task CreateProduct_NullManufacture_SuccessCreate()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 2,
            ProductName = "Test Product",
            Manufacture = null,
            CategoryId = 1
        };

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _productService.CreateProductAsync(product);

        // Assert
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Once);
    }
    [Test]
    public async Task CreateProductAsync_EmptyManufacture_SuccessCreate()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 2,
            ProductName = "Test Product",
            CategoryId = 1
        };

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _productService.CreateProductAsync(product);

        // Assert
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Once);
    }
    [Test]
    public async Task UpdateProduct_ExistProduct_UpdateProductAndSaveChanges()
    {
        // Arrange
        var product = new Product { ProductId = 1, ProductName = "Test", Manufacture = "Test", CategoryId = 1 };
        var existingProduct = new Product { ProductId = 1, ProductName = "Old", Manufacture = "Old", CategoryId = 1 };

        _mockUnitOfWork.Setup(u => u.ProductRepository.GetByIdAsync(product.ProductId))
            .ReturnsAsync(existingProduct);

        _mockUnitOfWork.Setup(u => u.SaveAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _productService.UpdateProductAsync(product);

        // Assert
        Assert.That(existingProduct.ProductName, Is.EqualTo(product.ProductName));
        Assert.That(existingProduct.Manufacture, Is.EqualTo(product.Manufacture));
        Assert.That(existingProduct.CategoryId, Is.EqualTo(product.CategoryId));

        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
    [Test]
    public void UpdateProduct_ProductNotExist_ThrowException()
    {
        // Arrange
        var product = new Product { ProductId = 1, ProductName = "Test", Manufacture = "Test", CategoryId = 1 };

        _mockUnitOfWork.Setup(u => u.ProductRepository.GetByIdAsync(product.ProductId))!
            .ReturnsAsync((Product)null!);

        // Act and assert
        var exception = Assert.ThrowsAsync<Exception>(() => _productService.UpdateProductAsync(product));
        Assert.That(exception.Message, Is.EqualTo($"The product with Id {product.ProductId} does not exist."));
    }
    [Test]
    public async Task DeleteProduct_ProductExists_DeleteProduct()
    {
        // Arrange
        var product = new Product { ProductId = 1, ProductName = "Test", Manufacture = "Test", CategoryId = 1 };

        _mockUnitOfWork.Setup(u => u.ProductRepository.GetByIdAsync(product.ProductId))
            .ReturnsAsync(product);

        /*_mockUnitOfWork.Setup(u => u.ProductRepository.DeleteAsync(product.ProductId))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveAsync())
            .Returns(Task.CompletedTask);*/
        // Act
        await _productService.DeleteProductAsync(product.ProductId);

        // Assert
        _mockUnitOfWork.Verify(u => u.ProductRepository.DeleteAsync(product.ProductId), Times.Once);
    }

    [Test]
    public async Task DeleteProduct_ProductDoesNotExist_NotDeleteAnyProduct()
    {
        // Arrange
        var id = 2;

        _mockUnitOfWork.Setup(u => u.ProductRepository.GetByIdAsync(id))
            .ReturnsAsync((Product)null);
        
        // Act and assert
        _mockUnitOfWork.Verify(u => u.ProductRepository.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
/*
public class ProductServiceTests
{

    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<IProductStore<Product>> _mockProductRepository;
    private IProductService _productService;
    private List<Product> _listProducts;
    private Mock<StoreContext> _mockContext;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockContext = new Mock<StoreContext>();
        _productService = new ProductService(_mockUnitOfWork.Object);
        _mockProductRepository = new Mock<IProductStore<Product>>();
        _listProducts = new List<Product>()
        {
            new Product() { ProductId = 1, ProductName = "Television", Manufacture = "Viet Nam", CategoryId = 1 },
            new Product() { ProductId = 2, ProductName = "Phone", Manufacture = "Viet Nam", CategoryId = 1 },
            new Product() { ProductId = 3, ProductName = "Session", Manufacture = "Viet Nam", CategoryId = 1 }
        };
    }

    [Test]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetAllAsync()).ReturnsAsync(_listProducts);

        // Act
        var result = await _productService.GetAllProductsAsync() as List<Product>;

        // Assert
        Assert.That(3, Is.EqualTo(result.Count));
    }

    [Test]
    public async Task GetProductById_ReturnsProductWithGivenId()
    {
        // Arrange
        var product = new Product
            { ProductId = 1, ProductName = "Product 1", Manufacture = "Viet Nam", CategoryId = 1 };
        _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.That(result, Is.EqualTo(product));
    }

    //Create Product 3 case: doesn't exist category, return normal, return error
    //Update successful, Update unsuccessful
    [Test]
    public async Task CreateProductAsync_ValidProduct_ReturnsSuccess()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            Manufacture = "Test Manufacture",
            CategoryId = 1
        };

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _productService.CreateProductAsync(product);

        // Assert
        //_mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task CreateProductAsync_ValidProduct_CreatesProductAndSavesChanges()
    {
// Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            Manufacture = "Test Manufacture",
            CategoryId = 1
        };

// Set up the mock CategoryRepository to return a valid category for the product
        var category = new Category { CategoryId = 1, CategoryName = "Test Category" };
        _mockUnitOfWork.Setup(cr => cr.CategoryRepository.GetByIdAsync(product.CategoryId)).ReturnsAsync(category);

// Act
        await _productService.CreateProductAsync(product);

// Assert
// Verify that the ProductRepository created the product
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Once);

// Verify that the UnitOfWork saved the changes
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task CreateProductAsync_InvalidCategory_ThrowsException()
    {
        // Arrange
        var product = new Product
        {
            ProductName = "Test Product",
            CategoryId = -1,
            Manufacture = "Test Manufacture"
        };
        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(null as Category);

        // Act & Assert
        //Assert.ThrowsAsync<Exception>(() => _productService.CreateProductAsync(product));
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Never);
    }

    [Test]
    public async Task CreateProductAsync_ValidProduct_ReturnsNull()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 2,
            ProductName = "Test Product",
            Manufacture = "Test Manufacture",
            CategoryId = 1
        };

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(product.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _productService.CreateProductAsync(product);

        // Assert
        _mockProductRepository.Verify(pr => pr.CreateAsync(product), Times.Never);
        //_mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}
[Test]
public async Task CreateProductAsync_NullProduct_ThrowsException()
{
    // Arrange

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.CreateProductAsync(null));
}

// Add more tests for other methods in the ProductService class
}*/