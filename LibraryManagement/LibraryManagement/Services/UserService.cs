using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using X.PagedList;

namespace LibraryManagement.Services;

public class UserService : IUserService
{
    private readonly UnitOfWork _unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IPagedList<User>> GetAllUsersAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var users = await _unitOfWork.UserRepository.GetAll();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            users = users.Where(u => u.UserName.Contains(searchTerm));
        }

        int totalCount = users.Count();

        var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new StaticPagedList<User>(pagedUsers, pageNumber, pageSize, totalCount);
    }

    public User GetUserById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0", nameof(id));
        }

        return _unitOfWork.UserRepository.GetById(id);
    }

    public string GetUserRole(User user)
    {
        return _unitOfWork.UserRepository.GetUserRole(user);
    }
 
    public User GetUserByUserNameAndPassword(string userName, string password)
    {
     return _unitOfWork.UserRepository.GetByUserNameAndPassword(userName, password);
    }
    public async Task<User> CreateUser(User user)
    {
        return await _unitOfWork.UserRepository.CreateAsync(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        return await _unitOfWork.UserRepository.UpdateAsync(user);
    }

    public async Task DeleteUser(int userId)
    {
        await _unitOfWork.UserRepository.DeleteAsync(userId);
    }

    public async Task<User> ChangePassword(int userId, string newPassword)
    {
        return await _unitOfWork.UserRepository.ChangePassword(userId, newPassword);
    }
 }

