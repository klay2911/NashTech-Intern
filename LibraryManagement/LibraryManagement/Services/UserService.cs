using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class UserService : IUserService
{
    private readonly UnitOfWork _unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public User GetUserById(int id)
    {
        return _unitOfWork.UserRepository.GetUserById(id);
    }

    public User GetUserByUserNameAndPassword(string userName, string password)
    {
        return _unitOfWork.UserRepository.GetUserByUserNameAndPassword(userName, password);
    }
}

