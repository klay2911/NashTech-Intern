using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Newtonsoft.Json;

namespace LibraryManagement.Controllers;

public class BorrowingRequestController : Controller
{
    private readonly IBookService _bookService;
    private readonly IBookBorrowingRequestService _borrowingRequestService;
    private readonly IBookBorrowingRequestDetailsService _borrowingRequestDetailsService;
    private readonly ILogger<BorrowingRequestController> _logger;

    public BorrowingRequestController(IBookService bookService, IBookBorrowingRequestService borrowingRequestService,
        IBookBorrowingRequestDetailsService borrowingRequestDetailsService, ILogger<BorrowingRequestController> logger)
    {
        _bookService = bookService;
        _borrowingRequestDetailsService = borrowingRequestDetailsService;
        _borrowingRequestService = borrowingRequestService;
        _logger = logger;
    }

    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public async Task<IActionResult> Index(int? page, string searchTerm)
    {
        page ??= 1;
        const int pageSize = 5;
        var pageNumber = (int)page;
        var books = await _bookService.GetAllBooksAsync(includeCategory: true);
        if (!string.IsNullOrEmpty(searchTerm))
        {
            books = books.Where(b => b.Title.Contains(searchTerm));
        }

        var bookIdsInRequest = HttpContext.Session.GetString("BookIdsInRequest");
        ViewBag.BookIdsInRequest = (bookIdsInRequest != null
            ? JsonConvert.DeserializeObject<List<int>>(bookIdsInRequest)
            : new List<int>())!;
        return View(books.ToPagedList(pageNumber, pageSize));
    }

    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public IActionResult AddBookToRequest(int bookId)
    {
        var bookIdsInRequest = HttpContext.Session.GetString("BookIdsInRequest");
        var list = new List<int>();
        if (bookIdsInRequest != null)
        {
            list = JsonConvert.DeserializeObject<List<int>>(bookIdsInRequest);
        }

        if (list is { Count: >= 5 })
        {
            TempData["Warning"] = "You cannot add more than 5 books to a request.";

            return RedirectToAction("Index");
        }

        list?.Add(bookId);

        HttpContext.Session.SetString("BookIdsInRequest", JsonConvert.SerializeObject(list));

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public IActionResult RemoveBookFromRequest(int bookId)
    {
        var bookIdsInRequest = HttpContext.Session.GetString("BookIdsInRequest");
        var list = new List<int>();
        if (bookIdsInRequest != null)
        {
            list = JsonConvert.DeserializeObject<List<int>>(bookIdsInRequest);
        }

        list?.Remove(bookId);

        HttpContext.Session.SetString("BookIdsInRequest", JsonConvert.SerializeObject(list));

        return RedirectToAction("Index");
    }

[Authorize(Roles = "NormalUser")]
[HttpPost]
public async Task<IActionResult> CreateBorrowingRequest()
{
    var userId = int.Parse(HttpContext.Session.GetString("UserId"));
    var userRequestsThisMonth = await _borrowingRequestService.GetNumberRequests(userId);

    if (userRequestsThisMonth >= 3)
    {
        TempData["Warning"] = "You have requested 3 times this month, you can request later next month.";
        return RedirectToAction("Index");
    }

    var bookIdsInRequestJson = HttpContext.Session.GetString("BookIdsInRequest");
    if (string.IsNullOrEmpty(bookIdsInRequestJson))
    {
        TempData["Warning"] = "You have not added any book to the request.";
        return RedirectToAction("Index");
    }

    var bookIdsInRequest = JsonConvert.DeserializeObject<List<int>>(bookIdsInRequestJson);
    if (bookIdsInRequest == null || !bookIdsInRequest.Any())
    {
        TempData["Warning"] = "You have not added any book to the request.";
        return RedirectToAction("Index");
    }

    foreach (var bookId in bookIdsInRequest)
    {
        var existingRequestDetail = await _borrowingRequestDetailsService.GetRequestDetail(bookId,userId);
        if (existingRequestDetail != null)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            TempData["Warning"] = $"You have already requested the book {book.Title}.";
            return RedirectToAction("Index");
        }
    }

    var borrowingRequest = new BookBorrowingRequest
    {
        UserId = userId,
        DateRequested = DateTime.Now,
        Status = "Pending"
    };

    await _borrowingRequestService.CreateBorrowingRequestAsync(borrowingRequest);

    foreach (var requestDetail in bookIdsInRequest.Select(bookId => new BookBorrowingRequestDetails
             {
                 RequestId = borrowingRequest.RequestId,
                 BookId = bookId
             }))
    {
        try
        {
            await _borrowingRequestDetailsService.AddBookToRequest(requestDetail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to add books to request.");
        }
    }

    HttpContext.Session.Remove("BookIdsInRequest");

    return RedirectToAction("Index");
}


        
    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public async Task<IActionResult> GetBorrowedBooks(int? page, string searchTerm)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var borrowingRequests = await _borrowingRequestService.GetUserRequests(Convert.ToInt32(userId));

        var borrowedBooks = new List<BookViewModel>();

        foreach (var request in borrowingRequests)
        {
            foreach (var detail in request.BookBorrowingRequestDetails)
            {
                var book = await _bookService.GetBookByIdAsync(detail.BookId);
                if (!string.IsNullOrEmpty(searchTerm) && !book.Title.Contains(searchTerm))
                {
                    continue;
                }
                borrowedBooks.Add(new BookViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    CategoryId = book.CategoryId,
                    Status = request.Status
                });
            }
        }

        int pageSize = 5;
        int pageNumber = (page ?? 1);
        return View(borrowedBooks.ToPagedList(pageNumber, pageSize));
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> ViewBorrowingRequests(int? page)
    {
        const int pageSize = 5;
        var pageNumber = (page ?? 1);
        var borrowingRequests = await _borrowingRequestService.GetAllBorrowingRequests();
        return View(borrowingRequests.ToPagedList(pageNumber, pageSize));
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> DetailsRequest(int requestId)
    {
        var borrowingRequest = await _borrowingRequestService.GetRequest(requestId);

        if (borrowingRequest == null)
        {
            return NotFound();
        }

        var requestDetails = new List<BookViewModel>();

        foreach (var detail in borrowingRequest.BookBorrowingRequestDetails)
        {
            var book = await _bookService.GetBookByIdAsync(detail.BookId);
            requestDetails.Add(new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                CategoryId = book.CategoryId,
                Status = borrowingRequest.Status
            });
        }

        return View(requestDetails);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> ApproveRequest(int requestId)
    {
        var librarianId = HttpContext.Session.GetString("UserId");
        await _borrowingRequestService.UpdateRequestStatus(requestId, "Approved", Convert.ToInt32(librarianId));
        return RedirectToAction("ViewBorrowingRequests");
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> RejectRequest(int requestId)
    {
        var librarianId = HttpContext.Session.GetString("UserId");
        await _borrowingRequestService.UpdateRequestStatus(requestId, "Rejected",Convert.ToInt32(librarianId));
        return RedirectToAction("ViewBorrowingRequests");
    }
}