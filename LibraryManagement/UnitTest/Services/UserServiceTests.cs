using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Moq;

namespace UnitTest.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<UnitOfWork> _mockUnitOfWork;
    private Mock<UserRepository> _mockUserRepository;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<UnitOfWork>();
        _mockUserRepository = new Mock<UserRepository>();
        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);
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
    public void GetUserById_InvalidId_ThrowsArgumentException()
    {
        // Arrange
        int invalidId = -1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _userService.GetUserById(invalidId));
        Assert.That(ex.ParamName, Is.EqualTo("id"));
    }

    [Test]
    public void GetUserById_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserById(1)).Throws(new Exception());

        // Act & Assert
        Assert.Throws<Exception>(() => _userService.GetUserById(1));
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
    [Test]
    public void GetUserByUserNameAndPassword_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.UserRepository.GetUserByUserNameAndPassword("test", "password")).Throws(new Exception());

        // Act & Assert
        Assert.Throws<Exception>(() => _userService.GetUserByUserNameAndPassword("test", "password"));
    }
}
