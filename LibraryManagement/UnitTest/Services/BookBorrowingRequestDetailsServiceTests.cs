using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class BookBorrowingRequestDetailsServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<BookBorrowingRequestDetailsRepository> _mockRequestDetailsRepository;
    private BookBorrowingRequestDetailsService _requestDetailsService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockRequestDetailsRepository = new Mock<BookBorrowingRequestDetailsRepository>();
        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestDetailsRepository).Returns(_mockRequestDetailsRepository.Object);
        _requestDetailsService = new BookBorrowingRequestDetailsService(_mockUnitOfWork.Object);
    }
    //Case CheckExistingRequest_IfBookRequestedWasAcceptedOrPending_ReturnException
    //Case CheckExistingRequest_IfBookRequestedWasRejected_ReturnRequestedSuccessful
    // [Test]
    // public async Task CheckExistingRequest_IfBookRequestedWasAcceptedOrPending_ReturnException()
    // {
    //     // Arrange
    //     var userId = 1;
    //     var bookIds = new List<int> { 1, 2 };
    //     _mockRequestDetailsRepository.Setup(repo => repo.GetRequestDetailByBookIdAndUserId(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new BookBorrowingRequestDetails());
    //
    //     // Act & Assert
    //     Assert.ThrowsAsync<Exception>(async () => await _requestDetailsService.CheckExistingRequest(userId, bookIds));
    // }
    //
    // [Test]
    // public async Task CheckExistingRequest_IfBookRequestedWasRejected_ReturnRequestedSuccessful()
    // {
    //     // Arrange
    //     var userId = 1;
    //     var bookIds = new List<int> { 1, 2 };
    //     _mockRequestDetailsRepository.Setup(repo => repo.GetRequestDetailByBookIdAndUserId(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((BookBorrowingRequestDetails)null);
    //     _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Book());
    //
    //     // Act
    //     var result = await _requestDetailsService.CheckExistingRequest(userId, bookIds);
    //
    //     // Assert
    //     Assert.That(result, Is.Not.Null);
    // }

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

    // [Test]
    // public async Task GetRequestDetail_ReturnDetail()
    // {
    //     // Arrange
    //     var expectedDetail = new BookBorrowingRequestDetails();
    //     int bookId = 1;
    //     int userId = 1;
    //     _mockUnitOfWork.Setup(u => u.BookBorrowingRequestDetailsRepository.GetRequestDetailByBookIdAndUserId(bookId, userId)).ReturnsAsync(expectedDetail);
    //
    //     // Act
    //     var result = await _requestDetailsService.GetRequestDetail(bookId, userId);
    //
    //     // Assert
    //     Assert.That(result, Is.EqualTo(expectedDetail));
    // }
}
