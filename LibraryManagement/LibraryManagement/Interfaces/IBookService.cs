using LibraryManagement.Models;
using X.PagedList;

namespace LibraryManagement.Interfaces;

public interface IBookService
{
    Task<IPagedList<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string searchTerm = "",
        bool includeCategory = false);
    // Task<IEnumerable<Book>> GetAllBooksAsync(bool includeCategory = false);
    Task<Book> GetBookByIdAsync(int id);
    Task CreateBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
}