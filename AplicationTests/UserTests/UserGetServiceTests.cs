using Aplication.Services;
using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.Interfaces.Logging;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories;
using Infrastracture.Logic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationTests.UserTests
{
    public class UserGetServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserGetService _service;
        private Mock<IAppLogger> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<IAppLogger>();
            _service = new UserGetService(_mockUserRepository.Object,_mockLogger.Object);
        }

        [Test]
        public async Task GetUsers_ShouldReturnUsers()
        {
            //Arrange
            List<UserPasswordDto> users = new List<UserPasswordDto>()
            {
                new UserPasswordDto(Guid.NewGuid(),"Se-biok","sebiok@gmail.com","123"),
                new UserPasswordDto(Guid.NewGuid(),"Se-biok2","sebiok2@gmail.com","1234")
            };
            _mockUserRepository.Setup(repo => repo.GetUsers()).ReturnsAsync(users);
            //Act
            var response = await  _service.GetUsers();
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(users[0].Id, response[0].Id, $"User ID should match");
            _mockUserRepository.Verify(provider => provider.GetUsers(), Times.Once, "GetUsers should be called once");
        }
        [Test]
        public async Task GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserPasswordDto(userId, "Se-biok", "sebiok@gmail.com", "123");
            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var response = await _service.GetUser(userId);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(user.Id, response.Id, "User ID should match");
            Assert.AreEqual(user.Login, response.Login, "Username should match");
            Assert.AreEqual(user.Email, response.Email, "Email should match");

            _mockUserRepository.Verify(provider => provider.GetUserById(userId), Times.Once, "GetUserById should be called once");
        }
        [Test]
        public async Task GetUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((UserPasswordDto?)null);

            // Act
            var response = await _service.GetUser(userId);

            // Assert
            Assert.IsNull(response, "Response should be null when user does not exist");

            _mockUserRepository.Verify(provider => provider.GetUserById(userId), Times.Once, "GetUserById should be called once");
        }
        [Test]
        public async Task GetUserByEmailOrLogin_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var login = "Se-biok";
            var user = new UserPasswordDto(Guid.NewGuid(), login, "sebiok@gmail.com", "123");
            _mockUserRepository.Setup(repo => repo.GetUserByEmailOrUsername(login)).ReturnsAsync(user);

            // Act
            var response = await _service.GetUserByEmailOrLogin(login);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(user.Id, response.Id, "User ID should match");
            Assert.AreEqual(user.Login, response.Login, "Username should match");
            Assert.AreEqual(user.Email, response.Email, "Email should match");

            _mockUserRepository.Verify(provider => provider.GetUserByEmailOrUsername(login), Times.Once, "GetUserByEmailOrUsername should be called once");
        }

        [Test]
        public async Task GetUserByEmailOrLogin_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var login = "Se-biok";
            _mockUserRepository.Setup(repo => repo.GetUserByEmailOrUsername(login)).ReturnsAsync((UserPasswordDto?)null);

            // Act
            var response = await _service.GetUserByEmailOrLogin(login);

            // Assert
            Assert.IsNull(response, "Response should be null when user does not exist");

            _mockUserRepository.Verify(provider => provider.GetUserByEmailOrUsername(login), Times.Once, "GetUserByEmailOrUsername should be called once");
        }
    }
}
