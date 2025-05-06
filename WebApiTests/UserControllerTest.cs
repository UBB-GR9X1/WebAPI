using ClassLibrary.Domain;
using ClassLibrary.IRepository;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests;

[TestClass]
public class UserControllerTest
{
    [TestMethod]
    public async Task GetAllUsers_WithValidController_ReturnsListOfUsers()
    {
        // Arrange
        var _mock_repo = new Mock<IUserRepository>();
        var _fake_users = new List<User>
        {
            new User { userId = 1, username = "john_sickko", password = "123456789", mail = "john@yahoo.com", role = "Patient", name = "John", birthDate = new DateOnly(1999, 03, 29), cnp = "1234567890123", address = "acasa locuiesc domle", phoneNumber = "07n-am cartela", registrationDate = new DateTime(2024, 03, 08, 19, 32, 0) },
            new User { userId = 2, username = "_xXx_paul_xXx_", password = "123456789", mail = "paul@yahoo.com", role = "Doctor", name = "John", birthDate = new DateOnly(2000, 02, 29), cnp = "1234567890123", address = "acasa locuiesc domle", phoneNumber = "07n-am cartela", registrationDate = new DateTime(2024, 03, 08, 19, 32, 0) }
        };
        _mock_repo.Setup(_repo => _repo.getAllUsersAsync()).ReturnsAsync(_fake_users);

        var _controller = new UserController(_mock_repo.Object);

        // Act
        var _result = await _controller.GetAllUsers();
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_users = _ok_result.Value as List<User>;
        Assert.AreEqual(2, _returned_users.Count);
    }

    [TestMethod]
    public async Task GetUserById_WithValidUserId_ReturnsUser()
    {
        // Arrange
        var _user_id = 1;
        var _mock_repo = new Mock<IUserRepository>();
        var _fake_user = new User { userId = _user_id, username = "john__sickko", password = "123456789", mail = "john@yahoo.com", role = "Patient", name = "John", birthDate = new DateOnly(1999, 03, 29), cnp = "1234567890123", address = "acasa locuiesc domle", phoneNumber = "07n-am cartela", registrationDate = new DateTime(2024, 03, 08, 19, 32, 0) };
        _mock_repo.Setup(repo => repo.getUserByIdAsync(_user_id)).ReturnsAsync(_fake_user);

        var _controller = new UserController(_mock_repo.Object);

        // Act
        var _result = await _controller.GetUserById(_user_id);
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_user = _ok_result.Value as User;
        Assert.AreEqual(_user_id, _returned_user.userId);
    }

    [TestMethod]
    public async Task CreateUser_WithValidUser_ReturnsCreatedAtAction()
    {
        // Arrange
        var _mock_repo = new Mock<IUserRepository>();
        var _user_id = 1;
        var _fake_user = new User { userId = _user_id, username = "john__sickko", password = "123456789", mail = "john@yahoo.com", role = "Patient", name = "John", birthDate = new DateOnly(1999, 03, 29), cnp = "1234567890123", address = "acasa locuiesc domle", phoneNumber = "07n-am cartela", registrationDate = new DateTime(2024, 03, 08, 19, 32, 0) };
        _mock_repo.Setup(_repo => _repo.addUserAsync(_fake_user)).Returns(Task.CompletedTask);

        var _controller = new UserController(_mock_repo.Object);

        // Act
        var _result = await _controller.CreateUser(_fake_user);
        var _created_at = _result as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(_created_at);
        Assert.AreEqual(_fake_user.userId, ((User)_created_at.Value).userId);
    }

    [TestMethod]
    public async Task DeleteUser_WithValidUserId_ReturnsNoContent()
    {
        // Arrange
        var _mock_repo = new Mock<IUserRepository>();
        int _user_id = 1;

        // No setup needed if the method succeeds (completes without exception)
        _mock_repo.Setup(_repo => repo.deleteUserAsync(_user_id)).Returns(Task.CompletedTask);

        var _controller = new UserController(_mock_repo.Object);

        // Act
        var _result = await _controller.DeleteUser(_user_id);

        // Assert
        Assert.IsInstanceOfType(_result, typeof(NoContentResult));
        _mock_repo.Verify(_repo => _repo.deleteUserAsync(_user_id), Times.Once);
    }
}
