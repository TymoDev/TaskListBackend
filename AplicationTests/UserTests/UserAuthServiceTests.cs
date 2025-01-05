using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Responce;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories;
using Infrastracture.Logic;
using Moq;

namespace AplicationTests.UserTests
{
    public class UserAuthServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IJwtProvider> _mockJwtProvider;
        private UserAuthService _userAuthService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _userAuthService = new UserAuthService(_mockUserRepository.Object, _mockPasswordHasher.Object, _mockJwtProvider.Object);
        }

        [Test]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new RegisterUserRequest("validUser", "StrongPassword123", "user@example.com");

            var userProp = new UserRequestHash(Guid.NewGuid(), "validUser", "user@example.com", "hashedPassword");
            var userResPass = new UserResponcePassword(Guid.NewGuid(), "validUser", "user@example.com", "hashedPassword");

            _mockPasswordHasher.Setup(hasher => hasher.Generate(request.Password)).Returns("hashedPassword");
            _mockUserRepository.Setup(repo => repo.CreateUser(userProp)).ReturnsAsync(userProp.Id);
            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(userResPass);
            _mockJwtProvider.Setup(provider => provider.GenerateAuthenticateToken(userResPass)).Returns("jwtToken");

            // Act
            var result = await _userAuthService.Register(userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Success, "Result should indicate success");
            Assert.AreEqual("jwtToken", result.Token, "The token should match the expected value");

            _mockPasswordHasher.Verify(hasher => hasher.Generate(request.Password), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateUser(It.IsAny<UserRequestHash>()), Times.Once);
            _mockJwtProvider.Verify(provider => provider.GenerateAuthenticateToken(It.IsAny<UserResponcePassword>()), Times.Once);
        }

        [Test]
        public async Task Register_ShouldReturnError_WhenUsernameIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new RegisterUserRequest("", "StrongPassword123", "user@example.com");

            // Act
            var result = await _userAuthService.Register(userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Username can not be longer than 30 symlos, or lower than 3", result.ErrorMessage, "Error message should indicate invalid username");
        }
        [Test]
        public async Task Register_ShouldReturnError_WhenPasswordIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new RegisterUserRequest("131212", "user@example.com", "st");

            // Act
            var result = await _userAuthService.Register(userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Weak password", result.ErrorMessage, "Error message should indicate invalid username");
        }
        /*[Test]
        public async Task Register_ShouldReturnError_WhenEmailIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new RegisterUserRequest("123331", "StrongPassword123", "");

            // Act
            var result = await _userAuthService.Register(userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Username can not be longer than 30 symlos, or lower than 3", result.ErrorMessage, "Error message should indicate invalid username");
        }*/
        [Test]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var request = new LoginUserRequest("validUser", "WrongPassword123");


            var userResponse = new UserResponcePassword(Guid.NewGuid(), "validUser","t@email.com", "hashedPassword");
            _mockUserRepository.Setup(repo => repo.GetUserByEmailOrUsername(request.Login)).ReturnsAsync(userResponse);
            _mockPasswordHasher.Setup(hasher => hasher.Verify(request.Password, userResponse.Password)).Returns(true);
            _mockJwtProvider.Setup(provider => provider.GenerateAuthenticateToken(userResponse)).Returns("jwtToken");

            // Act
            var result = await _userAuthService.Login(request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Success, "Result should indicate success");
            Assert.AreEqual("jwtToken", result.Token, "The token should match the expected value");

            _mockUserRepository.Verify(repo => repo.GetUserByEmailOrUsername(request.Login), Times.Once);
            _mockPasswordHasher.Verify(hasher => hasher.Verify(request.Password, userResponse.Password), Times.Once);
            _mockJwtProvider.Verify(provider => provider.GenerateAuthenticateToken(userResponse), Times.Once);
        }

        [Test]
        public async Task Login_ShouldReturnError_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new LoginUserRequest("validUser", "WrongPassword123");

            _mockUserRepository.Setup(repo => repo.GetUserByEmailOrUsername(request.Login)).ReturnsAsync((UserResponcePassword)null);

            // Act
            var result = await _userAuthService.Login(request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("BadRequest with data", result.ErrorMessage, "Error message should indicate invalid credentials");

            _mockUserRepository.Verify(repo => repo.GetUserByEmailOrUsername(request.Login), Times.Once);
            _mockPasswordHasher.Verify(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Login_ShouldReturnError_WhenPasswordIsIncorrect()
        {
            // Arrange
            var request = new LoginUserRequest("validUser", "WrongPassword123");

            var userResponse = new UserResponcePassword(Guid.NewGuid(), "validUser", "hashedPassword", "123313131321");

            _mockUserRepository.Setup(repo => repo.GetUserByEmailOrUsername(request.Login)).ReturnsAsync(userResponse);
            _mockPasswordHasher.Setup(hasher => hasher.Verify(request.Password, userResponse.Password)).Returns(false);

            // Act
            var result = await _userAuthService.Login(request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Incorrect password", result.ErrorMessage, "Error message should indicate incorrect password");

            _mockUserRepository.Verify(repo => repo.GetUserByEmailOrUsername(request.Login), Times.Once);
            _mockPasswordHasher.Verify(hasher => hasher.Verify(request.Password, userResponse.Password), Times.Once);
        }
    }
}