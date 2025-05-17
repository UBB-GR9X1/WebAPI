using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using WinUI.Repository;
using WinUI.Service;
using WinUI.Model;

namespace WinUi_Test.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<ILogInRepository> _mockRepo;
        private AuthService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ILogInRepository>();
            _service = new AuthService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task loadUserByUsername_validUsername_setsUserInfo()
        {
            // Arrange
            var expectedUser = new UserAuthModel(
                user_id: 1,
                username: "testuser",
                password: "pass123",
                mail: "test@mail.com",
                role: "user"
            );

            _mockRepo.Setup(r => r.getUserByUsername("testuser"))
                     .ReturnsAsync(expectedUser);

            // Act
            var result = await _service.loadUserByUsername("testuser");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedUser.userId, _service.allUserInformation.userId);
            Assert.AreEqual(expectedUser.username, _service.allUserInformation.username);
            Assert.AreEqual(expectedUser.password, _service.allUserInformation.password);
        }

        [TestMethod]
        public async Task verifyPassword_incorrectPassword_returnsFalse()
        {
            // Arrange
            var expectedUser = new UserAuthModel(1, "user", "correctPass", "mail@mail.com", "user");

            _mockRepo.Setup(r => r.getUserByUsername("user"))
                     .ReturnsAsync(expectedUser);

            await _service.loadUserByUsername("user");

            // Act
            var result = await _service.verifyPassword("wrongPass");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task verifyPassword_correctPassword_returnsTrue()
        {
            // Arrange
            var expectedUser = new UserAuthModel(1, "user", "correctPass", "mail@mail.com", "user");

            _mockRepo.Setup(r => r.getUserByUsername("user"))
                     .ReturnsAsync(expectedUser);

            _mockRepo.Setup(r => r.authenticationLogService(1, ActionType.LOGIN))
                     .ReturnsAsync(true);

            await _service.loadUserByUsername("user");

            // Act
            var result = await _service.verifyPassword("correctPass");

            // Assert
            Assert.IsTrue(result);
        }
    }
}
