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
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;
    private readonly IBookBorrowingRequestService _borrowingRequestService;
    private readonly IBookBorrowingRequestDetailsService _borrowingRequestDetailsService;
    private readonly ILogger<BorrowingRequestController> _logger;

    public BorrowingRequestController(IBookService bookService, ICategoryService categoryService,
        IUserService userService, IBookBorrowingRequestService borrowingRequestService,
        IBookBorrowingRequestDetailsService borrowingRequestDetailsService, ILogger<BorrowingRequestController> logger)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _userService = userService;
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

        if (list.Count >= 5)
        {
            TempData["Warning"] = "You cannot add more than 5 books to a request.";

            return RedirectToAction("Index");
        }

        list.Add(bookId);

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

        list.Remove(bookId);

        HttpContext.Session.SetString("BookIdsInRequest", JsonConvert.SerializeObject(list));

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public async Task<IActionResult> CreateBorrowingRequest()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRequestsThisMonth = await _borrowingRequestService.GetRequestsByUserThisMonth(Convert.ToInt32(userId));

        if (userRequestsThisMonth >= 3)
        {
            TempData["Warning"] = "You have requested 3 times this month, you can request later next month.";
            return RedirectToAction("Index");
        }

        var borrowingRequest = new BookBorrowingRequest
        {
            UserId = Convert.ToInt32(userId),
            DateRequested = DateTime.Now,
            Status = "Pending"
        };

        await _borrowingRequestService.CreateAsync(borrowingRequest);

        var bookIdsInRequestJson = HttpContext.Session.GetString("BookIdsInRequest");
        if (string.IsNullOrEmpty(bookIdsInRequestJson))
        {
            TempData["Warning"] = "You have not added any book to the request.";
            return RedirectToAction("Index");
        }

        var bookIdsInRequest = JsonConvert.DeserializeObject<List<int>>(bookIdsInRequestJson);
        foreach (var bookId in bookIdsInRequest)
        {
            var requestDetail = new BookBorrowingRequestDetails
            {
                RequestId = borrowingRequest.RequestId,
                BookId = bookId
            };

            try
            {
                await _borrowingRequestDetailsService.AddBookToRequest(requestDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add book with ID {bookId} to request.");
            }
        }

        HttpContext.Session.Remove("BookIdsInRequest");

        return RedirectToAction("Index");
    }
}