using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _userService = new UserService(_mockUnitOfWork.Object);
    }

    [Test]
    public void GetUserById_UserExists_ReturnsUser()
    {
        var user = new User { UserId = 1 , UserName = "TestUserName", Password = "TestPassWord"};
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserById(1)).Returns(user);

        var result = _userService.GetUserById(1);

        Assert.That(result, Is.EqualTo(user));
    }

    [Test]
    public void GetUserById_UserDoesNotExist_ReturnsNull()
    {
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserById(1)).Returns((User)null);

        var result = _userService.GetUserById(1);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetUserByUserNameAndPassword_ValidCredentials_ReturnsUser()
    {
        var user = new User { UserName = "test", Password = "password" };
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserByUserNameAndPassword("test", "password")).Returns(user);

        var result = _userService.GetUserByUserNameAndPassword("test", "password");

        Assert.That(result, Is.EqualTo(user));
    }

    [Test]
    public void GetUserByUserNameAndPassword_InvalidCredentials_ReturnsNull()
    {
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserByUserNameAndPassword("test", "wrongpassword")).Returns((User)null);

        var result = _userService.GetUserByUserNameAndPassword("test", "wrongpassword");

        Assert.That(result, Is.Null);
    }
}
