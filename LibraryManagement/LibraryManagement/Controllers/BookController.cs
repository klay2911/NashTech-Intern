using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace LibraryManagement.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly UnitOfWork _unitOfWork;

    public BookController(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    [Authorize(Roles = "SuperUser")]
    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        page ??= 1;
        const int pageSize = 3;
        var pageNumber = (int)page;
        var books = await _bookService.GetAllBooksAsync(includeCategory: true);
        return View(books.ToPagedList(pageNumber, pageSize));
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
    public async Task<IActionResult> Create(Book book)
    {
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
        if (book == null)
            return NotFound();
        return View(book);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.BookId)
            return NotFound();
        if (ModelState.IsValid)
        {
            await _bookService.UpdateBookAsync(book);
            return RedirectToAction("Index");
        }
        return View(book);
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
    
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> Viewing(int? page)
    {
        page ??= 1;
        const int pageSize = 3;
        var pageNumber = (int)page;
        var books = await _bookService.GetAllBooksAsync(includeCategory: true);
        return View(books.ToPagedList(pageNumber, pageSize));
    }
    /*[HttpGet]
   public async Task<IActionResult> Details(int id)
   {
       var book = await _bookService.GetBookByIdAsync(id);
       if (book == null)
           return NotFound();
       return View(book);
   }*/
}