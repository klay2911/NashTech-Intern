using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

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

    // [Authorize(Roles = "SuperUser")]
    // [HttpGet]
    // public async Task<IActionResult> Index(int? page)
    // {
    //     page ??= 1;
    //     const int pageSize = 3;
    //     var pageNumber = (int)page;
    //     var books = await _bookService.GetAllBooksAsync(includeCategory: true);
    //     return View(books.ToPagedList(pageNumber, pageSize));
    // }
    [Authorize(Roles = "SuperUser")]
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

        return View(books.ToPagedList(pageNumber, pageSize));
    }
    [HttpGet]
    public async Task<IActionResult> Create()    
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _bookService.CreateBookAsync(book);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.BookId)
            return NotFound();
        if (!ModelState.IsValid) return View(book);
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
/*[HttpGet]
 public async Task<IActionResult> Details(int id)
 {
     var book = await _bookService.GetBookByIdAsync(id);
     if (book == null)
         return NotFound();
     return View(book);
 }*/