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
    [Test]
    public async Task GetAllBorrowingRequests_ReturnsAllRequests()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> { new BookBorrowingRequest(), new BookBorrowingRequest() };
        _mockRequestRepository.Setup(repo => repo.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(1,2);

        // Assert
        Assert.AreEqual(requests, result);
    }
    [Test]
    public async Task GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnCorrectNumber()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> { new BookBorrowingRequest(), new BookBorrowingRequest(),new BookBorrowingRequest() };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnWrongNumber()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> { new BookBorrowingRequest(), new BookBorrowingRequest(), new BookBorrowingRequest(), new BookBorrowingRequest() };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        // Assert
        Assert.That(result, Has.Count.Not.EqualTo(1));
    }

    [Test]
    public async Task GetAllBorrowingRequests_RequestsExistInPage2_ReturnExactRequests()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> 
        { 
            new() {RequestId = 1, UserId = 1}, 
            new() {RequestId = 2, UserId = 1}, 
            new() {RequestId = 3, UserId = 2}
        };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.FirstOrDefault().RequestId, Is.EqualTo(3));
        Assert.That(result.FirstOrDefault().UserId, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllBorrowingRequests_RequestNotExistInPage2_ReturnEmpty()
    {
        // Arrange
        var requests = new List<BookBorrowingRequest> { new() {RequestId = 1, UserId = 1}, new() {RequestId = 2, UserId = 1} };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        // Assert
        Assert.That(result, Is.Empty);
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
    public async Task GetUserBorrowedBooks_GetNumberOfBooksInPage2_ReturnCorrectNumber()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 2;
        var searchTerm = "";
        var requests = new List<BookBorrowingRequest> { new(), new(), new(), new() };
        _mockRequestRepository.Setup(repo => repo.GetAll()).ReturnsAsync(requests);

        // Act
        var result = await _borrowingRequestService.GetAllBorrowingRequests(pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }
    //Case GetUserBorrowedBooks_BookExistInPage2_ReturnCorrectBook
    //Case GetUserBorrowedBooks_BookNotExistInPage2_ReturnWrongBook
    //Case GetUserBorrowedBooks_SearchRightTitleOfBook_ReturnCorrectBook
    //Case GetUserBorrowedBooks_SearchWrongTitleOfBook_ReturnCorrectBook
    /*[Test]
    public async Task GetUserBorrowedBooks_BookExistInPage2_ReturnCorrectBook()
    {
        // Arrange
        int userId = 1;
        int pageNumber = 2;
        int pageSize = 10;
        string searchTerm = null;
        var expectedBook = new List<BookViewModel>
        {
            new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Harry Porter"},
        };

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        // Assert.That(result, Does.Contain(expectedBook));
    }

    [Test]
    public async Task GetUserBorrowedBooks_BookNotExistInPage2_ReturnWrongBook()
    {
        // Arrange
        int userId = 1;
        int pageNumber = 2;
        int pageSize = 10;
        string searchTerm = null;
        var wrongBook = new List<BookViewModel>
        {
            new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Harry Porter"},
        };

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result, Does.Not.Contain(wrongBook));
    }

    [Test]
    public async Task GetUserBorrowedBooks_SearchRightTitleOfBook_ReturnCorrectBook()
    {
        // Arrange
        int userId = 1;
        int pageNumber = 1;
        int pageSize = 10;
        
         string searchTerm = "Harry Potter"; // Change this according to your test data
        
         BookViewModel expectedBook = new BookViewModel
         {
             // TODO: Initialize with expected book data that matches the search term
         };

         // Act
         var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

         // Assert
         Assert.That(result, Does.Contain(expectedBook));
    }

    [Test]
    public async Task GetUserBorrowedBooks_SearchWrongTitleOfBook_ReturnCorrectBook()
    {
         // Arrange
         int userId = 1;
         int pageNumber = 1;
         int pageSize = 10;

          string searchTerm = "Lord of the Rings";

          BookViewModel wrongBook = new BookViewModel
          {
              // TODO: Initialize with wrong book data that does not match the search term
          };

          // Act
          var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

          // Assert
          Assert.That(result, Does.Not.Contain(wrongBook));
    }*/
    // [Test]
    // public async Task CreateBorrowingRequestAsync_BookIncludedRight_CreatesNewRequest()
    // {
    //     // Arrange
    //     var request = new BookBorrowingRequest();
    //     _mockRequestRepository.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(request);
    //
    //     // Act
    //     await _borrowingRequestService.CreateBorrowingRequestAsync();
    //
    //     // Assert
    //     _mockRequestRepository.Verify(repo => repo.CreateAsync(request), Times.Once);
    // }
    //Case CreateBorrowingRequestAsync_BookNotIncluded_ReturnException
    //Case CreateBorrowingRequestAsync_GetNumberOfRequestsInPage2_ReturnCorrectNumber
    //Case CreateBorrowingRequestAsync_GetNumberOfRequestsInPage2_ReturnWrongNumber
    //Case CreateBorrowingRequestAsync_RequestExistInPage2_ReturnExactRequest
    //Case CreateBorrowingRequestAsync_RequestNotExistInPage2_ReturnWrongRequest


    [Test]
    public async Task CheckUserRequestLimit_RequestLessThan4_ReturnFalse()
    {
        // Arrange
        var userId = 1;
        _mockRequestRepository.Setup(repo => repo.GetRequestsByUserThisMonth(It.IsAny<int>())).ReturnsAsync(2);

        // Act
        var result = await _borrowingRequestService.CheckUserRequestLimit(userId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CheckUserRequestLimit_RequestWasNotReachLimit_ReturnTrue()
    {
        // Arrange
        var userId = 1;
        _mockRequestRepository.Setup(repo => repo.GetRequestsByUserThisMonth(It.IsAny<int>())).ReturnsAsync(4);

        // Act
        var result = await _borrowingRequestService.CheckUserRequestLimit(userId);

        // Assert
        Assert.That(result, Is.True);
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
    [Test]
    public async Task UpdateRequestStatus_ApprovedRequest_ReturnStatusApproved()
    {
        // Arrange
        var requestId = 1;
        var librarianId = 1;
        
        // Act
        await _borrowingRequestService.UpdateRequestStatus(requestId, "Approved", librarianId);

        // Assert
        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, "Approved", librarianId), Times.Once);
    }

    [Test]
    public async Task UpdateRequestStatus_RejectedRequest_ReturnStatusRejected()
    {
        // Arrange
        var requestId = 1;
        var librarianId = 1;
        
        // Act
        await _borrowingRequestService.UpdateRequestStatus(requestId, "Rejected", librarianId);

        // Assert
        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, "Rejected", librarianId), Times.Once);
    }

    [Test]
    public async Task UpdateRequestStatus_LibraryApprovedOrRejected_SetLibrarianId()
    {
        // Arrange
        var requestId = 1;
        var librarianId = 1;
        
        // Act
        await _borrowingRequestService.UpdateRequestStatus(requestId, "Approved", librarianId);

        // Assert
        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, It.IsAny<string>(), librarianId), Times.Once);
    }
}