using LibraryManagement.Interfaces;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LibraryContext _context;

    public UserRepository(LibraryContext context)
    {
        _context = context;
    }

    public User GetUserByUserNameAndPassword(string userName, string password)
    {
        return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
    }
    public User GetUserById(int id)
    {
        return _context.Users.Find(id);
    }
    // public int GetUserId()
    // {
    //     return UserId;
    // }
}
