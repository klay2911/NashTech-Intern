using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

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
        return  _context.Users.Find(id)!;
    }

    public virtual async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }


    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    public string GetUserRole(User user)
    {
        return user.Role.ToString();
    }
    public virtual async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public virtual async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public virtual async Task DeleteAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<User> ChangePassword(int userId, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);
        user.Password = newPassword;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
