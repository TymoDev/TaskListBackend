using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Core.ResultModels;
using Infrastracture.Caching;
using Infrastracture.EmailLogic;
using Infrastracture.Logic.CodesGeneration;
using Aplication.Services.User;
using Core.Interfaces.Logging;

[TestFixture]
public class UserResetPasswordServiceTests
{
    private Mock<ICodeGenerator> _mockCodeGenerator;
    private Mock<ICacher> _mockCacher;
    private Mock<IEmailSender> _mockEmailSender;
    private Mock<IAppLogger> _mockLogger;
    private UserResetPasswordService _service;

    [SetUp]
    public void Setup()
    {
        _mockCodeGenerator = new Mock<ICodeGenerator>();
        _mockCacher = new Mock<ICacher>();
        _mockEmailSender = new Mock<IEmailSender>();
        _mockLogger = new Mock<IAppLogger>();
        _service = new UserResetPasswordService(_mockCodeGenerator.Object, _mockCacher.Object, _mockEmailSender.Object,_mockLogger.Object);
    }

    [Test]
    public async Task ResetPasswordNotify_ShouldSendResetCode()
    {
        // Arrange
        var email = "test@example.com";
        var code = 123456;

        _mockCodeGenerator.Setup(cg => cg.GenerateSecureCode()).Returns(code);
        _mockCacher.Setup(c => c.SetCode(email, code)).ReturnsAsync(ResultModel.Ok());
        _mockEmailSender.Setup(es => es.SendMail(email, "Reset code mail", code.ToString())).Verifiable();

        // Act
        var result = await _service.ResetPasswordNotify(email);

        // Assert
        Assert.IsTrue(result.Success, "Result should indicate success");
        _mockCacher.Verify(c => c.SetCode(email, code), Times.Once, "SetCode should be called once");
        _mockEmailSender.Verify(es => es.SendMail(email, "Reset code mail", code.ToString()), Times.Once, "SendMail should be called once");
    }

    [Test]
    public async Task ResetPasswordVerify_ShouldReturnOk_WhenCodeMatches()
    {
        // Arrange
        var email = "test@example.com";
        var code = 123456;

        _mockCacher.Setup(c => c.GetCode(email)).ReturnsAsync(code);

        // Act
        var result = await _service.ResetPasswordVerify(email, code);

        // Assert
        Assert.IsTrue(result.Success, "Result should indicate success");
        _mockCacher.Verify(c => c.GetCode(email), Times.Once, "GetCode should be called once");
    }

    [Test]
    public async Task ResetPasswordVerify_ShouldReturnError_WhenCodeDoesNotMatch()
    {
        // Arrange
        var email = "test@example.com";
        var correctCode = 123456;
        var wrongCode = 654321;

        _mockCacher.Setup(c => c.GetCode(email)).ReturnsAsync(correctCode);

        // Act
        var result = await _service.ResetPasswordVerify(email, wrongCode);

        // Assert
        Assert.IsFalse(result.Success, "Result should indicate failure");
        Assert.AreEqual("Incorrect code", result.ErrorMessage, "Error message should be 'Incorrect code'");
        _mockCacher.Verify(c => c.GetCode(email), Times.Once, "GetCode should be called once");
    }

    [Test]
    public async Task ResetPasswordVerify_ShouldReturnError_WhenCodeIsNull()
    {
        // Arrange
        var email = "test@example.com";

        _mockCacher.Setup(c => c.GetCode(email)).ReturnsAsync((int?)null);

        // Act
        var result = await _service.ResetPasswordVerify(email, 123456);

        // Assert
        Assert.IsFalse(result.Success, "Result should indicate failure");
        Assert.AreEqual("Incorrect code", result.ErrorMessage, "Error message should be 'Incorrect code'");
        _mockCacher.Verify(c => c.GetCode(email), Times.Once, "GetCode should be called once");
    }
}
