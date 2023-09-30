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
    // public async Task<IEnumerable<Book>> GetAllBooksAsync(bool includeCategory = false)
    // {
    //     if (includeCategory)
    //     {
    //         return await _unitOfWork.BookRepository.GetAll().Include(b => b.Category).OrderBy(b =>b.Title).ToListAsync();
    //     }
    //     return await _unitOfWork.BookRepository.GetAll().ToListAsync();
    // }
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
            throw new Exception($"The CategoryId {book.CategoryId} does not exist.");
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