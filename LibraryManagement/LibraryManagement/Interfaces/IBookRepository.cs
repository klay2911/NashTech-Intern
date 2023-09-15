using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IBookRepository
{
    IQueryable<Book> GetAll();
    Task<Book> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book entity);
    Task DeleteAsync(int id);
}