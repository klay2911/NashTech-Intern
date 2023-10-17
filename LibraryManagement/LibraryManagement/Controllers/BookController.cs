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
        var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.PdfFilePath.TrimStart('/'));
        await using var pdfStream = System.IO.File.OpenRead(pdfPath);
        var pdf = new PdfDocument();
        pdf.LoadFromStream(pdfStream);

        var image = pdf.SaveAsImage(0);

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{book.BookId}Page1.png");

        image.Save(imagePath, ImageFormat.Png);

        ViewBag.BookImage = $"/images/{book.BookId}Page1.png";
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
    public async Task<IActionResult> Create(Book book, IFormFile pdfFile)
    {
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

        if (pdfFile is { Length: > 0 })
        {
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
        await _bookService.DeleteBookAsync(id);
        return RedirectToAction("Index");
    }
} 