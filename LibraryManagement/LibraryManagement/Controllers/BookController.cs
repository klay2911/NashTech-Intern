using System.Drawing.Imaging;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Spire.Pdf;

namespace LibraryManagement.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public BookController(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> Index(int? page, string searchTerm)
    {
        int pageNumber = (page ?? 1);
        int pageSize = 5;
    
        var books = await _bookService.GetAllBooksAsync(pageNumber, pageSize, searchTerm, includeCategory: true);
        
        ViewBag.SearchTerm = searchTerm;
    
        return View(books);
    }

    [Authorize(Roles = "NormalUser")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)  
    {  
        var book = await _bookService.GetBookByIdAsync(id);  
        var categories = await _categoryService.GetAllCategoriesAsync();  
        var bookIdsInRequest = HttpContext.Session.GetString("BookIdsInRequest");  
        ViewBag.BookIdsInRequest = (bookIdsInRequest != null  
            ? JsonConvert.DeserializeObject<List<int>>(bookIdsInRequest)  
            : new List<int>())!;  
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");  

        return View(book);  
    }

    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> Create()    
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View();
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> Create(Book book, IFormFile pdfFile, IFormFile coverFile)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
        var coverDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "covers");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        if (!Directory.Exists(coverDirectoryPath))
        {
            Directory.CreateDirectory(coverDirectoryPath);
        }

        var filePath = Path.Combine(directoryPath, pdfFile.FileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await pdfFile.CopyToAsync(stream);
        }
        book.PdfFilePath = $"/pdfs/{pdfFile.FileName}";

        var coverFilePath = Path.Combine(coverDirectoryPath, coverFile.FileName);
        await using (var stream = new FileStream(coverFilePath, FileMode.Create))
        {
            await coverFile.CopyToAsync(stream);
        }
        book.Cover = $"/covers/{coverFile.FileName}";

        await _bookService.CreateBookAsync(book);
        return RedirectToAction("Index");
    }
    
    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View(book);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> Update(int id, Book book, IFormFile pdfFile)
    {
        if (id != book.BookId)
            return NotFound();

        var oldBook = await _bookService.GetBookByIdAsync(id);

        if (pdfFile is { Length: > 0 })
        {
            if (!string.IsNullOrEmpty(oldBook.PdfFilePath))
            {
                var oldPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldBook.PdfFilePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPdfPath))
                {
                    System.IO.File.Delete(oldPdfPath);
                }
            }

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, pdfFile.FileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            book.PdfFilePath = $"/pdfs/{pdfFile.FileName}";
        }

        await _bookService.UpdateBookAsync(book);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();
        return View(book);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);

        if (!string.IsNullOrEmpty(book.PdfFilePath))
        {
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.PdfFilePath.TrimStart('/'));
            if (System.IO.File.Exists(pdfPath))
            {
                System.IO.File.Delete(pdfPath);
            }
        }
        if (!string.IsNullOrEmpty(book.Cover))
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.Cover.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        await _bookService.DeleteBookAsync(id);
        return RedirectToAction("Index");
    }
} 