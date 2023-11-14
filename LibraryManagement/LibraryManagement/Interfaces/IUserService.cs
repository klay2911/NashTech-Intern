using LibraryManagement.Models;
using X.PagedList;

namespace LibraryManagement.Interfaces;

public interface IUserService
{
    Task<IPagedList<User>> GetAllUsersAsync(int pageNumber, int pageSize, string searchTerm = "");
    User GetUserById(int id);
    User GetUserByUserNameAndPassword(string userName, string password);
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task DeleteUser(int userId);
    Task<User> ChangePassword(int userId, string newPassword);
}
