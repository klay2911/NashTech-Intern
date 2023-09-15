using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IUserService
{
    User GetUserById(int id);
    User GetUserByUserNameAndPassword(string userName, string password);
}
