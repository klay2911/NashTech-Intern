using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class BookServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<BookRepository> _mockBookRepository;
    private BookService _bookService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockBookRepository = new Mock<BookRepository>();
        _mockUnitOfWork.Setup(uow => uow.BookRepository).Returns(_mockBookRepository.Object);
        _bookService = new BookService(_mockUnitOfWork.Object);
    }

    [Test]
    public virtual async Task GetAllBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = new List<Book> { new Book(), new Book(), new Book(), new Book() };
        _mockBookRepository.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(1, 2);

        // Assert
        Assert.That(result.TotalItemCount, Is.EqualTo(4));
    }

    [Test]
    public async Task GetAllBooks_CountAllBooksInPage2_ReturnExactNumberOfBooks()
    {
        // Arrange
        var books = new List<Book> { new Book(), new Book(), new Book(), new Book() };
        _mockBookRepository.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(2, 2);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllBooks_TitleOfBookExistInPage2_ReturnExactBook()
    {
        // Arrange
        var books = new List<Book> { new() { Title = "Book1" }, new Book { Title = "Book2" }, new Book { Title = "Book3" }, new Book { Title = "Book4" } };
        _mockBookRepository.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(2, 2);

        // Assert
        Assert.That(result[0].Title, Is.EqualTo("Book3"));
    }

    [Test]
    public async Task GetAllBooks_BookNotExistInPage2_ReturnWrongBook()
    {
        // Arrange
        var books = new List<Book> { new() { Title = "Book1" }, new() { Title = "Book2" }, new() { Title = "Book3" }, new() { Title = "Book4" } };
        _mockBookRepository.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(2, 2);

        // Assert
        Assert.That(result[0].Title, Is.Not.EqualTo("Book1"));
    }
    
    [Test]
    public async Task GetBookById_BookExists_ReturnsBookWithGivenId()
    {
        // Arrange
        var book = new Book
            { BookId = 1, Title = "Book 1", Author = "Test", ISBN = "Test1" };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _bookService.GetBookByIdAsync(1);

        // Assert
        Assert.That(result, Is.EqualTo(book));
    }
    [Test]
    public async Task GetBookById_BookDoesNotExist_ReturnsNull()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(1)).ReturnsAsync((Book)null);

        // Act
        var result = await _bookService.GetBookByIdAsync(1);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task CreateBookAsync_ValidBook_ReturnsSuccess()
    {
        // Arrange
        var book = new Book
            { BookId = 1, Title = "Book 1", Author = "Test", ISBN = "Test1" ,CategoryId = 1};

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(book.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _bookService.CreateBookAsync(book);

        // Assert
        _mockBookRepository.Verify(pr => pr.CreateAsync(book), Times.Once);
    }
    [Test]
    public async Task CreateBookAsync_NullCategory_ThrowsException()
    {
        // Arrange
        var book = new Book
            { BookId = 1, Title = "Book 1", Author = "Test", ISBN = "Test1" ,CategoryId = 1};
        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(book.CategoryId)) .ReturnsAsync(null as Category);

        // Act & Assert
        // return Task.CompletedTask;
        var exception = Assert.ThrowsAsync<Exception>(() => _bookService.CreateBookAsync(book));
        Assert.That(exception.Message, Is.EqualTo($"The Category {book.CategoryId} no longer exists, please refresh the page."));
        _mockBookRepository.Verify(pr => pr.CreateAsync(book), Times.Never);
    }
    [Test]
    public async Task CreateBook_NullAuthor_SuccessCreate()
    {
        // Arrange
        var book = new Book
            { BookId = 1, Title = "Book 1", Author = null!, ISBN = "Test1" ,CategoryId = 1};

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(book.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _bookService.CreateBookAsync(book);

        // Assert
        _mockBookRepository.Verify(pr => pr.CreateAsync(book), Times.Once);
    }
    [Test]
    public async Task CreateBookAsync_EmptyAuthor_SuccessCreate()
    {
        // Arrange
        var book = new Book
            { BookId = 1, Title = "Book 1", ISBN = "Test1" ,CategoryId = 1};

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository.GetByIdAsync(book.CategoryId))
            .ReturnsAsync(new Category());

        // Act
        await _bookService.CreateBookAsync(book);

        // Assert
        _mockBookRepository.Verify(pr => pr.CreateAsync(book), Times.Once);
    }

    [Test]
    public async Task UpdateBookAsync_BookExists_UpdatesSuccessfully()
    {
        // Arrange
        var book = new Book { BookId = 1, Title = "Book 1", Author = "Author 1", ISBN = "ISBN1", CategoryId = 1 };
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(book.BookId)).ReturnsAsync(book);

        // Act
        await _bookService.UpdateBookAsync(book);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
    [Test]
    public void UpdateBookAsync_BookDoesNotExist_ThrowsException()
    {
        // Arrange
        var book = new Book { BookId = 1, Title = "Book 1", Author = "Author 1", ISBN = "ISBN1", CategoryId = 1 };
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(book.BookId)).ReturnsAsync((Book)null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _bookService.UpdateBookAsync(book));
    }

    [Test]
    public async Task UpdateBookAsync_BookExists_PropertiesUpdatedCorrectly()
    {
        // Arrange
        var existingBook = new Book { BookId = 1, Title = "Book 1", Author = "Author 1", ISBN = "ISBN1", CategoryId = 1 };
        var updatedBook = new Book { BookId = 1, Title = "Updated Title", Author = "Updated Author", ISBN = "Updated ISBN", CategoryId = 2 };
    
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(updatedBook.BookId)).ReturnsAsync(existingBook);

        // Act
        await _bookService.UpdateBookAsync(updatedBook);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(existingBook.Title, Is.EqualTo(updatedBook.Title));
            Assert.That(existingBook.Author, Is.EqualTo(updatedBook.Author));
            Assert.That(existingBook.ISBN, Is.EqualTo(updatedBook.ISBN));
            Assert.That(existingBook.CategoryId, Is.EqualTo(updatedBook.CategoryId));
        });
    }
    
    [Test]
    public async Task DeleteBookAsync_BookExists_DeletesSuccessfully()
    {
        // Arrange
        var id = 1;
        _mockUnitOfWork.Setup(uow => uow.BookRepository.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        await _bookService.DeleteBookAsync(id);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.BookRepository.DeleteAsync(id), Times.Once);
    }
    [Test]
    public void DeleteBookAsync_BookDoesNotExist_ThrowsException()
    {
        // Arrange
        var id = 1;
        _mockUnitOfWork.Setup(uow => uow.BookRepository.DeleteAsync(id)).ThrowsAsync(new Exception());

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _bookService.DeleteBookAsync(id));
    }
}