using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services;

public class BookService: IBookService
{
    private readonly UnitOfWork _unitOfWork;

    public BookService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Book>> GetAllBooksAsync(bool includeCategory = false)
    {
        return await _unitOfWork.BookRepository.GetAllAsync(includeCategory);
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
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(book.CategoryId);
        if (existingBook == null)
        {
            throw new Exception($"The Book with Title {book.Title} does not exist.");
        }

        if (category == null)
        {
            throw new Exception($"The Category {book.CategoryId} no longer exists, please refresh the page.");
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