using LibraryManagement.Models;

namespace LibraryManagement.Interfaces;

public interface IUserRepository
{
    User GetUserById(int id);
    User GetUserByUserNameAndPassword(string userName, string password);
}

