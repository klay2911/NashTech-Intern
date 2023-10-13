using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.Controllers;

[Authorize(Roles = "SuperUser")]
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

    [HttpGet]
    public async Task<IActionResult> Create()    
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View();
    }
    
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
        // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs", pdfFile.FileName);
        // await using (var stream = new FileStream(filePath, FileMode.Create))
        // {
        //     await pdfFile.CopyToAsync(stream);
        // }

        book.PdfFilePath = $"/pdfs/{pdfFile.FileName}";
    
        await _bookService.CreateBookAsync(book);
        return RedirectToAction("Index");
    }

    
    // [HttpPost]
    // public async Task<IActionResult> Create(Book book)
    // {
    //     await _bookService.CreateBookAsync(book);
    //     return RedirectToAction("Index");
    // }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, Book book, IFormFile pdfFile)
    {
        if (id != book.BookId)
            return NotFound();
        // if (!ModelState.IsValid) return View(book);

        if (pdfFile != null && pdfFile.Length > 0)
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