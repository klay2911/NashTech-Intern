using LibraryManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace LibraryManagement.Controllers;

public class BorrowingRequestController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IBookBorrowingRequestService _borrowingRequestService;
    private readonly IBookBorrowingRequestDetailsService _borrowingRequestDetailsService;

    public BorrowingRequestController(IBookService bookService,ICategoryService categoryService ,IBookBorrowingRequestService borrowingRequestService, IBookBorrowingRequestDetailsService borrowingRequestDetailsService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _borrowingRequestDetailsService = borrowingRequestDetailsService;
        _borrowingRequestService = borrowingRequestService;
    }
    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public async Task<IActionResult> Index(int? page, string searchTerm)
    {
        page ??= 1;
        const int pageSize = 3;
        var pageNumber = (int)page;
        var books = await _bookService.GetAllBooksAsync(includeCategory: true);
        if (!string.IsNullOrEmpty(searchTerm))
        {
            books = books.Where(b => b.Title.Contains(searchTerm));
        }
        //var requestId = int.Parse(RouteData.Values["id"].ToString());
        // var booksInRequest = new List<int>();
        //
        // var enumerable = books.ToList();
        // foreach (var book in enumerable)
        // {
        //     try
        //     {
        //         //var requestDetails = await _borrowingRequestDetailsService.GetBorrowingRequestDetailsByRequestIdAndBookIdAsync(requestId, book.BookId);
        //         booksInRequest.Add(book.BookId);
        //     }
        //     catch (InvalidOperationException)
        //     {
        //         // The book is not in the request
        //     }
        // }
        // ViewBag.BooksInRequest = booksInRequest;
        return View(books.ToPagedList(pageNumber,pageSize));
    }
}