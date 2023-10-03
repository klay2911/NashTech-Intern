using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class BookBorrowingRequestTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<BookBorrowingRequestRepository> _mockRequestRepository;
    private BookBorrowingRequestService _borrowingRequestService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockRequestRepository = new Mock<BookBorrowingRequestRepository>();
        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository).Returns(_mockRequestRepository.Object);
        _borrowingRequestService = new BookBorrowingRequestService(_mockUnitOfWork.Object);
    }
    [Test]
    public async Task GetAllBorrowingRequests_ReturnsAllRequests()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> { new BookBorrowingRequest(), new BookBorrowingRequest() };
        _mockRequestRepository.Setup(repo => repo.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests();

        // Assert
        Assert.AreEqual(requests, result);
    }

    [Test]
    public async Task GetRequest_ReturnsCorrectRequest()
    {
        // Arrange
        var requestId = 1;
        var request = new BookBorrowingRequest();
        _mockRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);

        // Act
        var result = await _borrowingRequestService.GetRequest(requestId);

        // Assert
        Assert.That(result, Is.EqualTo(request));
    }

    [Test]
    public async Task CreateBorrowingRequestAsync_BookIncluded_CreatesNewRequest()
    {
        // Arrange
        var request = new BookBorrowingRequest();
        _mockRequestRepository.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(request);

        // Act
        await _borrowingRequestService.CreateBorrowingRequestAsync(request);

        // Assert
        _mockRequestRepository.Verify(repo => repo.CreateAsync(request), Times.Once);
    }

    [Test]
    public async Task GetNumberRequests_ReturnsCorrectNumber()
    {
        // Arrange
        var userId = 1;
        var requestCount = 5;
        _mockRequestRepository.Setup(repo => repo.GetRequestsByUserThisMonth(userId)).ReturnsAsync(requestCount);

        // Act
        var result = await _borrowingRequestService.GetNumberRequests(userId);

        // Assert
        Assert.That(result, Is.EqualTo(requestCount));
    }

    [Test]
    public async Task UpdateRequestStatus_UpdatesCorrectly()
    {
        // Arrange
        var requestId = 1;
        var status = "Approved";
        var librarianId = 1;
        
        // Act
        await _borrowingRequestService.UpdateRequestStatus(requestId, status, librarianId);

        // Assert
        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, status, librarianId), Times.Once);
    }
}