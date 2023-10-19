using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class BookBorrowingRequestDetailsServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<BookRepository> _mockBookRepository;
    private Mock<BookBorrowingRequestDetailsRepository> _mockRequestDetailsRepository;
    private BookBorrowingRequestDetailsService _requestDetailsService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockBookRepository = new Mock<BookRepository>();
        _mockRequestDetailsRepository = new Mock<BookBorrowingRequestDetailsRepository>();
        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestDetailsRepository).Returns(_mockRequestDetailsRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.BookRepository).Returns(_mockBookRepository.Object);

        _requestDetailsService = new BookBorrowingRequestDetailsService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetAll_ReturnWhichBookBelongToWhatRequest()
    {
        // Arrange
        var expectedBooks = new List<BookBorrowingRequestDetails> { new BookBorrowingRequestDetails(), new BookBorrowingRequestDetails() };
        _mockUnitOfWork.Setup(u => u.BookBorrowingRequestDetailsRepository.GetAll()).ReturnsAsync(expectedBooks);

        // Act
        var result = await _requestDetailsService.GetAll();

        // Assert
        Assert.That(result, Is.EqualTo(expectedBooks));
    }

    [Test]
    public async Task GetBook_ReturnBook()
    {
        // Arrange
        var expectedBook = new BookBorrowingRequestDetails();
        int requestId = 1;
        int bookId = 1;
        _mockUnitOfWork.Setup(u => u.BookBorrowingRequestDetailsRepository.GetBookByRequestId(requestId, bookId)).ReturnsAsync(expectedBook);

        // Act
        var result = await _requestDetailsService.GetBook(requestId, bookId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedBook));
    }
    [Test]
    public async Task GetBook_WhenBookDoesNotExist_ReturnsNull()
    {
        // Arrange
        int requestId = 1;
        int bookId = 1;
        _mockUnitOfWork.Setup(u => u.BookBorrowingRequestDetailsRepository.GetBookByRequestId(requestId, bookId)).ReturnsAsync((BookBorrowingRequestDetails)null);

        // Act
        var result = await _requestDetailsService.GetBook(requestId, bookId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddBookToRequest_AddBook()
    {
        // Arrange
        var details = new BookBorrowingRequestDetails { BookId = 1 };
        var book = new Book();
        _mockUnitOfWork.Setup(u => u.BookRepository.GetByIdAsync(details.BookId)).ReturnsAsync(book);
        
        // Act
        await _requestDetailsService.AddBookToRequest(details);

        // Assert
        _mockUnitOfWork.Verify(u => u.BookBorrowingRequestDetailsRepository.Add(details), Times.Once);
    }

    [Test]
    public async Task CheckExistingRequest_BookHasRequestedBefore_ReturnsBook()
    {
        // Arrange
        var bookIds = new List<int> { 1 };
        var book = new Book() { BookId = 1};
    
        var request = new BookBorrowingRequest
        {
            Status = "Pending"
        };
    
        var userId = 1;
        var bookId = 1;
    
        _mockUnitOfWork.Setup(repo => repo.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookIdAndUserId(userId, bookId))
            .ReturnsAsync(new BookBorrowingRequestDetails
            {
                BookBorrowingRequest = request,
                BookId = book.BookId,
                Book = book,
            });
        _mockUnitOfWork.Setup(repo => repo.BookRepository.GetByIdAsync(1)).ReturnsAsync(new Book()
        {
            BookId = 1
        });
    
        // Act
        var result = await _requestDetailsService.CheckExistingRequest(userId, bookIds);
    
        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.BookId, Is.EqualTo(book.BookId));
    }
    
    [Test]
    public async Task CheckExistingRequest_RequestDoesNotExist_ReturnsNull()
    {
        // Arrange
        var userId = 1;
        var bookIds = new List<int> { 1, 2 };
        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookIdAndUserId(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((BookBorrowingRequestDetails)null);

        // Act
        var result = await _requestDetailsService.CheckExistingRequest(userId, bookIds);

        // Assert
        Assert.IsNull(result);
    }
}
