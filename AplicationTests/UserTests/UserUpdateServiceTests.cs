using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using Core.DTO.UserDTO.Request;
using Core.Interfaces.Repositories;
using Core.ResultModels;
using Core.ValidationModels.User;
using Infrastracture.Logic;
using Aplication.Services.User;
using Core.DTO.UserDTO;

[TestFixture]
public class UserUpdateServiceTests
{
    private Mock<IUserRepository> _mockRepository;
    private Mock<IPasswordHasher> _mockHasher;
    private UserUpdateService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockHasher = new Mock<IPasswordHasher>();
        _service = new UserUpdateService(_mockRepository.Object, _mockHasher.Object);
    }

    [Test]
    public async Task UpdateUser_ShouldReturnOk_WhenValidationSucceedsAndUserIsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UserRequestId(Guid.NewGuid(), "validUsername", "valid@example.com", "ValidPass123!");

        _mockHasher.Setup(h => h.Generate(request.Password)).Returns("hashedPassword");
        _mockRepository.Setup(r => r.UpdateUser(It.IsAny<UserRequestHash>())).ReturnsAsync(userId);

        // Act
        var result = await _service.UpdateUser(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success, "Result should indicate success");
        _mockRepository.Verify(r => r.UpdateUser(It.IsAny<UserRequestHash>()), Times.Once, "UpdateUser should be called once");
    }

    [Test]
    public async Task UpdateUser_ShouldReturnError_WhenUsernameValidationFails()
    {
        // Arrange
        var request = new UserRequestId(Guid.NewGuid(), "va", "valid@example.com", "ValidPass123!");

        // Act
        var result = await _service.UpdateUser(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success, "Result should indicate failure");
        Assert.AreEqual("Username can not be longer than 30 symlos, or lower than 3", result.ErrorMessage, "Error message should be 'Invalid username'");
        _mockRepository.Verify(r => r.UpdateUser(It.IsAny<UserRequestHash>()), Times.Never, "UpdateUser should not be called");
    }

    [Test]
    public async Task UpdateUser_ShouldReturnError_WhenPasswordValidationFails()
    {
        // Arrange
        var request = new UserRequestId(Guid.NewGuid(), "validUsername", "valid@example.com", "Vakh!");

        // Act
        var result = await _service.UpdateUser(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success, "Result should indicate failure");
        Assert.AreEqual("Weak password", result.ErrorMessage, "Error message should be 'Invalid password'");
        _mockRepository.Verify(r => r.UpdateUser(It.IsAny<UserRequestHash>()), Times.Never, "UpdateUser should not be called");
    }

    [Test]
    public async Task UpdateUser_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        // Arrange
        var request = new UserRequestId(Guid.NewGuid(), "validUsername", "valid@example.com", "ValidPass123!");

        _mockHasher.Setup(h => h.Generate(request.Password)).Returns("hashedPassword");
        _mockRepository.Setup(r => r.DeleteUser(It.IsAny<Guid>())).ReturnsAsync((Guid?)null);


        // Act
        var result = await _service.UpdateUser(request);

        // Assert
        Assert.IsNull(result, "Result should be null when repository returns null");
        _mockRepository.Verify(r => r.UpdateUser(It.IsAny<UserRequestHash>()), Times.Once, "UpdateUser should be called once");
    }

    [Test]
    public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteUser(userId)).ReturnsAsync(userId);

        // Act
        var result = await _service.DeleteUser(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success, "Result should indicate success");
        _mockRepository.Verify(r => r.DeleteUser(userId), Times.Once, "DeleteUser should be called once");
    }

    [Test]
    public async Task DeleteUser_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteUser(It.IsAny<Guid>())).ReturnsAsync((Guid?)null);

        // Act
        var result = await _service.DeleteUser(userId);

        // Assert
        Assert.IsNull(result, "Result should be null when repository returns null");
        _mockRepository.Verify(r => r.DeleteUser(userId), Times.Once, "DeleteUser should be called once");
    }
}
