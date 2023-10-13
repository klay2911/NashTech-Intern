using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

public class BookBorrowingRequestServiceTests
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
    /*[Test]
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

    //Case GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnCorrectNumber
    //Case GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnWrongNumber
    //Case GetAllBorrowingRequests_RequestExistInPage2_ReturnExactRequest
    //Case GetAllBorrowingRequests_RequestNotExistInPage2_ReturnWrongRequest

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
    //Case GetUserBorrowedBooks_GetNumberOfBooksInPage2_ReturnCorrectNumber
    //CaseGetUserBorrowedBooks_BookExistInPage2_ReturnCorrectBook
    //CaseGetUserBorrowedBooks_BookNotExistInPage2_ReturnWrongBook
    // [Test]
    // public async Task CreateBorrowingRequestAsync_BookIncludedRight_CreatesNewRequest()
    // {
    //     // Arrange
    //     var request = new BookBorrowingRequest();
    //     _mockRequestRepository.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(request);
    //
    //     // Act
    //     await _borrowingRequestService.CreateBorrowingRequestAsync(request);
    //
    //     // Assert
    //     _mockRequestRepository.Verify(repo => repo.CreateAsync(request), Times.Once);
    // }
    //Case CreateBorrowingRequestAsync_BookNotIncluded_ReturnException

    //Case CheckUserRequestLimit_RequestLessThan4_ReturnTrue
    //Case CheckUserRequestLimit_RequestWasNotReachLimit_ReturnFalse
    //Case UpdateRequestStatus_ApprovedRequest_ReturnStatusApproved
    //Case UpdateRequestStatus_RejectedRequest_ReturnStatusRejected
    //Case UpdateRequestStatus_LibraryApprovedOrRejected_SetLibrarianId
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
    }*/
}