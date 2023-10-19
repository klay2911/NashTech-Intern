using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;
using Newtonsoft.Json;

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
        var requests = new List<BookBorrowingRequest> { new(), new() };
        _mockRequestRepository.Setup(repo => repo.GetAll()).ReturnsAsync(requests);

        var result = await _borrowingRequestService.GetAllBorrowingRequests(1, 2);

        Assert.That(result, Is.EqualTo(requests));
    }

    [Test]
    public async Task GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnCorrectNumber()
    {
        var requests = new List<BookBorrowingRequest> { new(), new(), new() };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetAllBorrowingRequests_GetNumberOfRequestsInPage2_ReturnWrongNumber()
    {
        var requests = new List<BookBorrowingRequest> { new(), new(), new(), new() };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        Assert.That(result, Has.Count.Not.EqualTo(1));
    }

    [Test]
    public async Task GetAllBorrowingRequests_RequestsExistInPage2_ReturnExactRequests()
    {
        var requests = new List<BookBorrowingRequest>
        {
            new() { RequestId = 1, UserId = 1 },
            new() { RequestId = 2, UserId = 1 },
            new() { RequestId = 3, UserId = 2 }
        };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 2);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.FirstOrDefault().RequestId, Is.EqualTo(3));
        Assert.That(result.FirstOrDefault().UserId, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllBorrowingRequests_RequestNotExistInPage2_ReturnEmpty()
    {
        var requests = new List<BookBorrowingRequest>
            { new() { RequestId = 1, UserId = 1 }, new() { RequestId = 2, UserId = 1 } };
        _mockRequestRepository.Setup(r => r.GetAll()).ReturnsAsync(requests);

        var result = await _borrowingRequestService.GetAllBorrowingRequests(2, 3);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetRequest_ReturnsCorrectRequest()
    {
        var requestId = 1;
        var request = new BookBorrowingRequest();
        _mockRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);

        var result = await _borrowingRequestService.GetRequest(requestId);

        Assert.That(result, Is.EqualTo(request));
    }

    [Test]
    public async Task CreateBorrowingRequestAsync_BookIncludedRight_CreatesNewRequest()
    {
        // Arrange
        var borrowingRequest = new BookBorrowingRequest();
        var bookIdsInRequestJson = JsonConvert.SerializeObject(new List<int> { 1, 2, 3 });

        // Act
        await _borrowingRequestService.CreateBorrowingRequestAsync(borrowingRequest, bookIdsInRequestJson);

        // Assert
        _mockRequestRepository.Verify(r => r.CreateAsync(borrowingRequest), Times.Once);
    }

    [Test]
    public void CreateBorrowingRequestAsync_bookIdsInRequestJsonIsNullOrEmpty_ReturnException()
    {
        // Arrange
        var borrowingRequest = new BookBorrowingRequest();
        string bookIdsInRequestJson = null;

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() =>
            _borrowingRequestService.CreateBorrowingRequestAsync(borrowingRequest, bookIdsInRequestJson));
    }

    [Test]
    public void CreateBorrowingRequestAsync_bookIdsInRequestIsNull_ReturnException()
    {
        // Arrange
        var borrowingRequest = new BookBorrowingRequest();
        string bookIdsInRequestJson = JsonConvert.SerializeObject(null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() =>
            _borrowingRequestService.CreateBorrowingRequestAsync(borrowingRequest, bookIdsInRequestJson));
    }

    [Test]
    public async Task CheckUserRequestLimit_RequestLessThan4_ReturnFalse()
    {
        var userId = 1;
        _mockRequestRepository.Setup(repo => repo.GetRequestsByUserThisMonth(It.IsAny<int>())).ReturnsAsync(2);

        var result = await _borrowingRequestService.CheckUserRequestLimit(userId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CheckUserRequestLimit_RequestWasNotReachLimit_ReturnTrue()
    {
        var userId = 1;
        _mockRequestRepository.Setup(repo => repo.GetRequestsByUserThisMonth(It.IsAny<int>())).ReturnsAsync(4);

        var result = await _borrowingRequestService.CheckUserRequestLimit(userId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UpdateRequestStatus_UpdatesCorrectly()
    {
        var requestId = 1;
        var status = "Approved";
        var librarianId = 1;

        await _borrowingRequestService.UpdateRequestStatus(requestId, status, librarianId);

        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, status, librarianId), Times.Once);
    }

    [Test]
    public async Task UpdateRequestStatus_ApprovedRequest_ReturnStatusApproved()
    {
        var requestId = 1;
        var librarianId = 1;

        await _borrowingRequestService.UpdateRequestStatus(requestId, "Approved", librarianId);

        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, "Approved", librarianId), Times.Once);
    }

    [Test]
    public async Task UpdateRequestStatus_RejectedRequest_ReturnStatusRejected()
    {
        var requestId = 1;
        var librarianId = 1;

        await _borrowingRequestService.UpdateRequestStatus(requestId, "Rejected", librarianId);

        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, "Rejected", librarianId), Times.Once);
    }

    [Test]
    public async Task UpdateRequestStatus_LibraryApprovedOrRejected_SetLibrarianId()
    {
        var requestId = 1;
        var librarianId = 1;

        await _borrowingRequestService.UpdateRequestStatus(requestId, "Approved", librarianId);

        _mockRequestRepository.Verify(repo => repo.UpdateRequestStatus(requestId, It.IsAny<string>(), librarianId),
            Times.Once);
    }

    [Test]
    public async Task GetUserBorrowedBooks_ReturnsPagedList()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 1;
        var pageSize = 10;
        var searchTerm = "test";

        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            }
        };

        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            }
        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.TotalItemCount, Is.EqualTo(1));
    }

    [Test]
    public async Task GetUserBorrowedBooks_GetTotalOfBooks_ReturnCorrectNumber()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 1;
        var pageSize = 10;
        var searchTerm = "test";
        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            }
        };

        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            }
        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result.TotalItemCount, Is.EqualTo(1));
    }

    [Test]
    public async Task GetUserBorrowedBooks_ApprovedRequest_CallSaveAsync()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 1;
        var pageSize = 10;
        var searchTerm = "test";
        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            }
        };
        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            }
        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
    
    [Test]
    public async Task GetUserBorrowedBooks_BookExistInPage2_ReturnCorrectBook()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 2;
        var pageSize = 2;
        var searchTerm = "";
        
        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                new() { BookId = 2 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 3 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 4 }
                }
            }
        };
        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            },
            new() {BookId = 2, Title = "Book Test 1"}, new() { BookId = 3,Title = "Book Test 2"}, new() {BookId = 4, Title = "Harry Porter"}

        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result[0].Title, Is.EqualTo("Book Test 2"));
    }
    
    [Test]
    public async Task GetUserBorrowedBooks_BookExistInPage2_ReturnWrongBook()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 2;
        var pageSize = 2;
        var searchTerm = "";
        
        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                new() { BookId = 2 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 3 }
                }
            },
            new()
            {
                UserId = userId,
                Status = "Approved",
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 4 }
                }
            }
        };
        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            },
            new() {BookId = 2, Title = "Book Test 1"}, new() { BookId = 3,Title = "Book Test 2"}, new() {BookId = 4, Title = "Harry Porter"}

        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result[0].Title, Is.Not.EqualTo("Book Test 1"));
    }
    
    [Test]
    public async Task GetUserBorrowedBooks_SearchRightTitleOfBook_ReturnCorrectBook()
    {
        // Arrange
        var userId = 1;
        var pageNumber = 1;
        var pageSize = 2;
        var searchTerm = "test book";
        
        var mockRequests = new List<BookBorrowingRequest>
        {
            new()
            {
                UserId = userId,
                Status = "Approved",
                ExpiryDate = DateTime.Now.AddDays(10),
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
                {
                    new() { BookId = 1 }
                }
            }
        };
        var mockBooks = new List<Book>
        {
            new()
            {
                BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
                PdfFilePath = "path/to/pdf"
            }
        };

        _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
            .ReturnsAsync(mockRequests);
        _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));

        // Act
        var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);

        // Assert
        Assert.That(result.First().Title, Is.EqualTo("test book"));
    }

    // [Test]
    // public async Task GetUserBorrowedBooks_ApprovedRequest_AddExpiryDate()
    // {
    //     // Arrange
    //     var userId = 1;
    //     var pageNumber = 1;
    //     var pageSize = 10;
    //     var searchTerm = "";
    //     var mockRequests = new List<BookBorrowingRequest>
    //     {
    //         new()
    //         {
    //             UserId = userId,
    //             Status = "Approved",
    //             ExpiryDate = DateTime.Now.AddSeconds(10.04),
    //             BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
    //             {
    //                 new() { BookId = 1 }
    //             }
    //         }
    //     };
    //     var mockBooks = new List<Book>
    //     {
    //         new()
    //         {
    //             BookId = 1, Title = "test book", Author = "test author", ISBN = "1234567890", CategoryId = 1,
    //             PdfFilePath = "path/to/pdf"
    //         }
    //     };
    //
    //     _mockUnitOfWork.Setup(uow => uow.BookBorrowingRequestRepository.GetRequestsByUser(userId))
    //         .ReturnsAsync(mockRequests);
    //     _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(It.IsAny<int>()))
    //         .ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.BookId == id));
    //     
    //
    //     // Act
    //     var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);
    //
    //     // Assert
    //     Assert.That(result.First().ExpiryDate, Is.EqualTo(DateTime.Now.AddSeconds(10)));
    // }
}



//     var mockRequests = new List<BookBorrowingRequest>
    //     {
    //         new()
    //         {
    //             UserId = userId,
    //             Status = "Approved",
    //             ExpiryDate = DateTime.Now.AddDays(10),
    //             BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails>
    //             {
    //                 new()
    //                 {
    //                     BookId = 1,
    //                     Book = new Book
    //                     {
    //                         BookId = 1,
    //                         Title = "Test Book",
    //                         Author = "Test Author",
    //                         ISBN = "1234567890",
    //                         CategoryId = 1,
    //                         PdfFilePath = "testpath.pdf"
    //                     }
    //                 }
    //             }
    //         }
    //     };
    //
    

    //     // _mockUnitOfWork.Setup(u => u.BookBorrowingRequestRepository.GetRequestsByUser(1)).ReturnsAsync(expectedBook);
//     var wrongBook = new List<BookViewModel>
//         {
//             new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Book Test 1"}, new() {Title = "Harry Porter"},
//         };
//
//         
//         var result = await _borrowingRequestService.GetUserBorrowedBooks(userId, pageNumber, pageSize, searchTerm);
//
//         
//         Assert.That(result, Does.Not.Contain(wrongBook));
//     }
//
// string searchTerm = "Harry Potter"; // Change this according to your test data
//         
//          BookViewModel expectedBook = new BookViewModel
//          {
//              
//          };
//
//           string searchTerm = "Lord of the Rings";
          