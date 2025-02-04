using Aplication.Core.Models.TaskModel;
using BusinessLogic.Services;
using Castle.Core.Logging;
using Core.DTO.TaskDTO;
using Core.Interfaces.Logging;
using Core.ResultModels;
using DataAccess.Repositories.RepositoriesTb;
using Moq;

namespace AplicationTests.TasksTests
{
    public class TaskServiceTests
    {

        private Mock<ITaskListRepository> _mockTaskRepository;
        private Mock<IAppLogger> _mockLogger;
        private TaskListService _testService;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskListRepository>();
            _mockLogger = new Mock<IAppLogger>();
            _testService = new TaskListService(_mockTaskRepository.Object,_mockLogger.Object);
        }

        [Test]
        public async Task GetTasksTest_ShouldReturnCorrectArrayOfElements()
        {
            // Arrange
            var expectedTasks = new List<TaskResponse>
            {
                new TaskResponse(Guid.NewGuid(), "Task 1", "in-progress"),
                new TaskResponse(Guid.NewGuid(), "Task 2", "completed"),
                new TaskResponse(Guid.NewGuid(), "Task 3", "completed")
            };

            _mockTaskRepository.Setup(repo => repo.GetTasks()).ReturnsAsync(expectedTasks);

            // Act
            var result = await _testService.GetTasks();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(expectedTasks.Count, result.Count, "The count of tasks should match");

            for (int i = 0; i < expectedTasks.Count; i++)
            {
                Assert.AreEqual(expectedTasks[i].id, result[i].id, $"Task ID at index {i} should match");
                Assert.AreEqual(expectedTasks[i].TaskName, result[i].TaskName, $"Task Name at index {i} should match");
            }

            _mockTaskRepository.Verify(repo => repo.GetTasks(), Times.Once, "GetTasks should be called exactly once");
        }
        
        [Test]
        public async Task GetTask_ShouldReturnCorrectTask_WhenTaskExistsForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(taskId, "Task 1", "in-progress"),
                new TaskResponse(Guid.NewGuid(), "Task 2", "completed")
            };

            var expectedTask = new TaskResponse(taskId, "Task 1", "in-progress");

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);

            // Act
            var result = await _testService.GetTask(taskId, userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(expectedTask.id, result.id, "Task ID should match");
            Assert.AreEqual(expectedTask.TaskName, result.TaskName, "Task Name should match");
            Assert.AreEqual(expectedTask.taskStatus, result.taskStatus, "Task Status should match");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
        }

        [Test]
        public async Task GetTask_ShouldReturnNull_WhenTaskDoesNotExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(Guid.NewGuid(), "Task 1", "in-progress"),
                new TaskResponse(Guid.NewGuid(), "Task 2", "completed")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);

            // Act
            var result = await _testService.GetTask(taskId, userId);

            // Assert
            Assert.IsNull(result, "Result should be null when task is not found for the user");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.GetByIdTask(It.IsAny<Guid>()), Times.Never, "GetByIdTask should not be called when task is not found");
        }
        
        [Test]
        public async Task GetUserTasks_ShouldReturnTasks_WhenTasksExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var expectedTasks = new List<TaskResponse>
            {
                new TaskResponse(Guid.NewGuid(), "Task 1", "in-progress"),
                new TaskResponse(Guid.NewGuid(), "Task 2", "completed")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(expectedTasks);

            // Act
            var result = await _testService.GetUserTasks(userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(expectedTasks.Count, result.Count, "The count of tasks should match");

            for (int i = 0; i < expectedTasks.Count; i++)
            {
                Assert.AreEqual(expectedTasks[i].id, result[i].id, $"Task ID at index {i} should match");
                Assert.AreEqual(expectedTasks[i].TaskName, result[i].TaskName, $"Task Name at index {i} should match");
                Assert.AreEqual(expectedTasks[i].taskStatus, result[i].taskStatus, $"Task Status at index {i} should match");
            }

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
        }

        [Test]
        public async Task GetUserTasks_ShouldReturnNull_WhenNoTasksExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync((List<TaskResponse>)null);

            // Act
            var result = await _testService.GetUserTasks(userId);

            // Assert
            Assert.IsNull(result, "Result should be null when no tasks exist for the user");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
        }

        [Test]
        public async Task CreateTask_ShouldReturnOk_WhenTaskIsSuccessfullyCreated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new TaskRequest("New Task","in-progress");

            _mockTaskRepository
                .Setup(repo => repo.CreateTask(It.IsAny<Guid>(), request.TaskName, request.TaskStatus, userId))
                .ReturnsAsync(new TaskResponse(Guid.NewGuid(),"task","taskStatus"));

            // Act
            var result = await _testService.CreateTask(request, userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Success, "Result should indicate success");

            _mockTaskRepository.Verify(repo => repo.CreateTask(It.IsAny<Guid>(), request.TaskName, request.TaskStatus, userId), Times.Once, "CreateTask should be called exactly once");
        }

        [Test]
        public async Task CreateTask_ShouldReturnError_WhenTaskModelCreationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new TaskRequest("", "");

            // Act
            var result = await _testService.CreateTask(request, userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Task name and status is requared", result.ErrorMessage, "Result message should match the error message");

            _mockTaskRepository.Verify(
                repo => repo.CreateTask(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()),
                Times.Never,
                "CreateTask in repository should not be called when TaskModel creation fails");
        }
        [Test]
        public async Task UpdateTask_ShouldReturnOk_WhenTaskIsSuccessfullyUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var request = new TaskRequest("Updated Task", "completed");

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(taskId, "Existing Task", "in-progress")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);
            _mockTaskRepository.Setup(repo => repo.UpdateTask(taskId, request.TaskName, request.TaskStatus)).ReturnsAsync(new TaskResponse(Guid.NewGuid(), "task", "taskStatus"));

            // Act
            var result = await _testService.UpdateTask(taskId, userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Success, "Result should indicate success");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.UpdateTask(taskId, request.TaskName, request.TaskStatus), Times.Once, "UpdateTask should be called exactly once");
        }

        [Test]
        public async Task UpdateTask_ShouldReturnError_WhenTaskDoesNotExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var request = new TaskRequest("Updated Task", "completed");

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(new List<TaskResponse>());

            // Act
            var result = await _testService.UpdateTask(taskId, userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Sorry, but user don't have this task", result.ErrorMessage, "Error message should indicate missing task");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.UpdateTask(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never, "UpdateTask should not be called when task does not exist");
        }

        [Test]
        public async Task UpdateTask_ShouldReturnError_WhenTaskModelCreationFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var request = new TaskRequest("", "");

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(taskId, "Existing Task", "in-progress")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);

            // Act
            var result = await _testService.UpdateTask(taskId, userId, request);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Task name and status is requared", result.ErrorMessage, "Error message should match");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.UpdateTask(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never, "UpdateTask should not be called when TaskModel creation fails");
        }
        [Test]
        public async Task DeleteTask_ShouldReturnOk_WhenTaskIsSuccessfullyDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(taskId, "Existing Task", "in-progress")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);
            _mockTaskRepository.Setup(repo => repo.DeleteTask(taskId)).ReturnsAsync(taskId);

            // Act
            var result = await _testService.DeleteTask(taskId, userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.Success, "Result should indicate success");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.DeleteTask(taskId), Times.Once, "DeleteTask should be called exactly once");
        }

        [Test]
        public async Task DeleteTask_ShouldReturnError_WhenTaskDoesNotExistForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(new List<TaskResponse>());

            // Act
            var result = await _testService.DeleteTask(taskId, userId);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsFalse(result.Success, "Result should indicate failure");
            Assert.AreEqual("Sorry, but user don't have this task", result.ErrorMessage, "Error message should indicate missing task");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.DeleteTask(It.IsAny<Guid>()), Times.Never, "DeleteTask should not be called when task does not exist");
        }

        [Test]
        public async Task DeleteTask_ShouldReturnNull_WhenRepositoryFailsToDeleteTask()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var userTasks = new List<TaskResponse>
            {
                new TaskResponse(taskId, "Existing Task", "in-progress")
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserTask(userId)).ReturnsAsync(userTasks);
            _mockTaskRepository.Setup(repo => repo.DeleteTask(taskId)).ReturnsAsync((Guid?)null);

            // Act
            var result = await _testService.DeleteTask(taskId, userId);

            // Assert
            Assert.IsNull(result, "Result should be null when repository fails to delete task");

            _mockTaskRepository.Verify(repo => repo.GetByUserTask(userId), Times.Once, "GetByUserTask should be called exactly once");
            _mockTaskRepository.Verify(repo => repo.DeleteTask(taskId), Times.Once, "DeleteTask should be called exactly once");
        }
    }
}