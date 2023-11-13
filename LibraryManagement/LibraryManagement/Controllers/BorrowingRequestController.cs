using System.Drawing.Imaging;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spire.Pdf;
using X.PagedList;

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
        int pageNumber = (page ?? 1);
        const int pageSize = 5;
        var books = await _bookService.GetAllBooksAsync(pageNumber, pageSize, searchTerm, includeCategory: true);
        var bookIdsInRequest = HttpContext.Session.GetString("BookIdsInRequest");
        ViewBag.SearchTerm = searchTerm;
        ViewBag.BookIdsInRequest = (bookIdsInRequest != null ? JsonConvert.DeserializeObject<List<int>>(bookIdsInRequest) : new List<int>())!;
        
        var bookImages = new Dictionary<int, string>();
        foreach (var book in books)
        {
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.PdfFilePath.TrimStart('/'));
            await using var pdfStream = System.IO.File.OpenRead(pdfPath);
            var pdf = new PdfDocument();
            pdf.LoadFromStream(pdfStream);
            var image = pdf.SaveAsImage(0);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{book.BookId}Page1.png");
            bookImages.Add(book.BookId, $"/images/{book.BookId}Page1.png");
            var directoryPath = Path.GetDirectoryName(imagePath);
            if (!Directory.Exists(directoryPath))
            {
             Directory.CreateDirectory(directoryPath);
            }
            image.Save(imagePath, ImageFormat.Png);
        }
        ViewBag.BookImages = bookImages;

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
        if (await _borrowingRequestService.CheckUserRequestLimit(userId))
        {
            TempData["Warning"] = "You have requested 3 times this month, you can request later next month.";
            return RedirectToAction("Index");
        }
        
        var bookIdsInRequestJson = HttpContext.Session.GetString("BookIdsInRequest");
        List<int> bookIdsInRequest = string.IsNullOrEmpty(bookIdsInRequestJson) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(bookIdsInRequestJson);
        var book = await _borrowingRequestDetailsService.CheckExistingRequest(userId, bookIdsInRequest);
        if (book != null)
        {
            TempData["Warning"] = $"You have already requested the book {book.Title}.";
            return RedirectToAction("Index");
        }
        
        var borrowingRequest = new BookBorrowingRequest
        {
            UserId = userId,
            DateRequested = DateTime.Now,
            Status = "Pending"
        };

        try
        {
            await _borrowingRequestService.CreateBorrowingRequestAsync(borrowingRequest, bookIdsInRequestJson);
        }
        catch (Exception ex)
        {
            TempData["Warning"] = ex.Message;
            return RedirectToAction("Index");
        }

        foreach (var requestDetail in bookIdsInRequest.Select(bookId => new BookBorrowingRequestDetails
                 {
                     RequestId = borrowingRequest.RequestId,
                     BookId = bookId
                 }))
            await _borrowingRequestDetailsService.AddBookToRequest(requestDetail);
        HttpContext.Session.Remove("BookIdsInRequest");

        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public async Task<IActionResult> GetBorrowedBooks(int? page, string searchTerm)
    {
        var userId = HttpContext.Session.GetString("UserId");
        const int pageSize = 5;
        var pageNumber = (page ?? 1);
        var borrowedBooks = await _borrowingRequestService.GetUserBorrowedBooks(Convert.ToInt32(userId), pageNumber, pageSize, searchTerm);

        ViewBag.SearchTerm = searchTerm;
        return View(borrowedBooks);
    }
    
    [Authorize(Roles = "NormalUser")]
    [HttpPost]
    public async Task<IActionResult> SaveLastReadPageNumber(int bookId, int pageNumber)
    {
        var userId = HttpContext.Session.GetString("UserId");

        await _borrowingRequestDetailsService.SaveLastReadPageNumber(bookId, Convert.ToInt32(userId), pageNumber);
        return Ok();
    }

    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> ViewBorrowingRequests(int? page, string searchTerm = "")  
    {  
        const int pageSize = 5;  
        var pageNumber = (page ?? 1);
        var borrowingRequests = await _borrowingRequestService.GetAllBorrowingRequests(pageNumber, pageSize, searchTerm);  

        return View(borrowingRequests);
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> DetailsRequest(int requestId)
    {
        var borrowingRequest = await _borrowingRequestService.GetRequest(requestId);
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
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> ShowBorrowers(int id)
    {
        var users = await _borrowingRequestDetailsService.GetUsersBorrowingBook(id);
        return View(users);
    }
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public IActionResult Report()
    {
        return View();
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> Report([FromForm] ReportViewModel model)
    {
        ViewBag.ReportData = model.ReportType switch
        {
            "NumberOfReadersPerBook" => await _borrowingRequestDetailsService.GetNumberOfReadersPerBook(model.StartDate.Value, model.EndDate.Value),
            "TopReadBooksInMonth" => await _bookService.GetTopReadBooksInMonth(model.StartDate.Value.Year, model.StartDate.Value.Month),
            "BooksNotBorrowedInPeriod" => await _borrowingRequestDetailsService.GetBooksNotBorrowedInPeriod(model.StartDate.Value, model.EndDate.Value),
            "NumberOfBooks" => await _bookService.GetNumberOfBooks(), _ => ViewBag.ReportData
        };
        return View(model);
    }
}