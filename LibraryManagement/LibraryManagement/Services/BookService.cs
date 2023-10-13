using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using X.PagedList;

namespace LibraryManagement.Services;

public class BookService: IBookService
{
    private readonly UnitOfWork _unitOfWork;

    public BookService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IPagedList<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string searchTerm = "", bool includeCategory = false)
    {
        var books = await _unitOfWork.BookRepository.GetAllAsync(includeCategory);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            books = books.Where(b => b.Title.Contains(searchTerm));
        }

        int totalCount = books.Count();

        var pagedBooks = books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new StaticPagedList<Book>(pagedBooks, pageNumber, pageSize, totalCount);
    }
    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await _unitOfWork.BookRepository.GetByIdAsync(id);
    }
    
    public async Task CreateBookAsync(Book book)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(book.CategoryId);
        if (category == null)
        {
            throw new Exception($"The Category {book.CategoryId} no longer exists, please refresh the page.");
        }
        await _unitOfWork.BookRepository.CreateAsync(book);
    }
    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(book.BookId);
        if (existingBook == null)
        {
            throw new Exception($"The Book with Id {book.BookId} does not exist.");
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.ISBN = book.ISBN;
        existingBook.CategoryId = book.CategoryId;

        await _unitOfWork.SaveAsync();
    }
    
    public async Task DeleteBookAsync(int id)
    {
        await _unitOfWork.BookRepository.DeleteAsync(id);
    }
}