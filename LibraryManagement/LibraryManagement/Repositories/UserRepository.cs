using LibraryManagement.Interfaces;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LibraryContext _context;
    
    public UserRepository()
    {
    }
    public UserRepository(LibraryContext context)
    {
        _context = context;
    }

    public virtual User GetUserByUserNameAndPassword(string userName, string password)
    {
        return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
    }
    public virtual User GetUserById(int id)
    {
        return _context.Users.Find(id);
    }
}
