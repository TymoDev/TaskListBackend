
using BusinessLogic.Services;
using Core.DTO.TaskDTO;
using Core.ResultModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Text;

[TestFixture]
public class TaskControllerTests : IDisposable
{
    private WebApplicationFactory<Program> _factory;
    private Mock<ITaskListService> _taskServiceMock;

    [SetUp]
    public void Setup()
    {
        _taskServiceMock = new Mock<ITaskListService>();

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITaskListService));
                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }
                services.AddTransient(_ => _taskServiceMock.Object);
            });

           /* builder.Configure(app =>
            {
                app.UseJwtCookieMiddleware(); 
            });*/
        });
    }
    public void Dispose()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task GetTasks_ShouldReturnOkAndTasks()
    {
        // Arrange
        var mockTasks = new List<TaskResponse>
        {
            new TaskResponse(Guid.NewGuid(), "Task 1", "InProgress"),
            new TaskResponse(Guid.NewGuid(), "Task 2", "Completed")
        };

        _taskServiceMock.Setup(s => s.GetTasks()).ReturnsAsync(mockTasks);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/Tasks");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tasks = JsonConvert.DeserializeObject<List<TaskResponse>>(jsonResponse);

        Assert.AreEqual(mockTasks.Count, tasks.Count);
        for (int i = 0; i < mockTasks.Count; i++)
        {
            Assert.AreEqual(mockTasks[i].id, tasks[i].id);
            Assert.AreEqual(mockTasks[i].TaskName, tasks[i].TaskName);
            Assert.AreEqual(mockTasks[i].taskStatus, tasks[i].taskStatus);
        }
    }

    [Test]
    public async Task GetTask_ShouldReturnOkAndTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var mockTask = new TaskResponse(taskId, "Task 1", "InProgress");

        _taskServiceMock.Setup(s => s.GetTask(taskId, userId)).ReturnsAsync(mockTask);

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer mock-token");
        client.DefaultRequestHeaders.Add("userId", userId.ToString());

        // Act
        var response = await client.GetAsync($"api/tasks/{taskId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var task = JsonConvert.DeserializeObject<TaskResponse>(jsonResponse);

        Assert.IsNotNull(task);
        Assert.AreEqual(mockTask.id, task.id);
        Assert.AreEqual(mockTask.TaskName, task.TaskName);
        Assert.AreEqual(mockTask.taskStatus, task.taskStatus);
    }

    [Test]
    public async Task GetTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _taskServiceMock.Setup(s => s.GetTask(taskId, userId)).ReturnsAsync((TaskResponse)null);

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer mock-token");
        client.DefaultRequestHeaders.Add("userId", userId.ToString());

        // Act
        var response = await client.GetAsync($"api/tasks/{taskId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task CreateTask_ShouldReturnOkAndTaskId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockTaskId = Guid.NewGuid();

        var taskRequest = new TaskRequest("New Task", "Pending");

        _taskServiceMock.Setup(s => s.CreateTask(It.IsAny<TaskRequest>(), userId)).ReturnsAsync(ResultModelObject<TaskResponse>.Ok);

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer mock-token");
        client.DefaultRequestHeaders.Add("userId", userId.ToString());

        var requestContent = new StringContent(JsonConvert.SerializeObject(taskRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("api/tasks", requestContent);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createdTaskId = JsonConvert.DeserializeObject<Guid>(jsonResponse);

        Assert.AreEqual(mockTaskId, createdTaskId);
    }


}