using LibraryManagement.Interfaces;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public class UserRepository : ILibraryRepository<User>
{
    private readonly LibraryContext _context;
    
    public UserRepository()
    {
    }
    public UserRepository(LibraryContext context)
    {
        _context = context;
    }

    public virtual User GetByUserNameAndPassword(string userName, string password)
    {
        return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
    }
    public virtual User GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
    
}
