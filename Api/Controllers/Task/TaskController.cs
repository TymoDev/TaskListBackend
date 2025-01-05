using BusinessLogic.Services;
using Core.DTO.TaskDTO;
using Core.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Task
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService service;

        public TasksController(ITaskService service)
        {
            this.service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TaskResponse>>> GetTasks()
        {
            var tasks = await service.GetTasks();
            var responce = tasks.Select(r => new TaskResponse(r.id, r.TaskName, r.taskStatus));
            return Ok(responce);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<TaskResponse>> GetTask(Guid id)
        {
            var userId = User.FindFirst("userId")?.Value;
            var userIdGuid = new Guid(userId);
            var task = await service.GetTask(id, userIdGuid);
            if (task == null)
            {
                return NotFound();
            }
            var responce = new TaskResponse(task.id, task.TaskName, task.taskStatus);
            return Ok(responce);
        }
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<List<TaskResponse>>> GetUserTasks()
        {
            var userId = User.FindFirst("userId")?.Value;
            var userIdGuid = new Guid(userId);
            var tasks = await service.GetUserTasks(userIdGuid);
            if (tasks == null)
            {
                return NotFound();
            }
            var responce = tasks.Select(r => new TaskResponse(r.id, r.TaskName, r.taskStatus));
            return Ok(responce);
        }
             [Authorize]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] TaskRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
            var userIdGuid = new Guid(userId);
            var result = await service.CreateTask(request,userIdGuid);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);//Yagni we can return here id in result model, but right now its bullshit
        }
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateTask(Guid id, [FromBody] TaskRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
            var userIdGuid = new Guid(userId);
            var result = await service.UpdateTask(id, userIdGuid, request);
            if (result == null)
            {
                return NotFound();
            }
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(id);
        }
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ResultModel>> DeleteTask(Guid id)
        {
            var userId = User.FindFirst("userId")?.Value;
            var userIdGuid = new Guid(userId);
            var result = await service.DeleteTask(id, userIdGuid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
