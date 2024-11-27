using BusinessLogic.Services;
using Core.DTO.TaskDTO;
using Core.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Task
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService service;

        public TaskController(ITaskService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<TaskResponse>>> GetTasks()
        {
            var tasks = await service.GetTasks();
            var responce = tasks.Select(r => new TaskResponse(r.id, r.TaskName, r.taskStatus));
            return Ok(responce);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskResponse>> GetTask(Guid id)
        {
            var task = await service.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }
            var responce = new TaskResponse(task.id, task.TaskName, task.taskStatus);
            return Ok(responce);
        }
        //   [Authorize]
        /*[HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<List<TaskResponse>>> GetUserTasks(Guid userId)
        {
            var tasks = await service.GetUserTasks(userId);
            if (tasks == null)
            {
                return NotFound();
            }
            var responce = tasks.Select(r => new TaskResponse(r.id, r.TaskName, r.taskStatus));
            return Ok(responce);
        }*/
        //     [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] TaskRequest request)
        {
            Guid id = Guid.NewGuid();
            var result = await service.CreateTask(request);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(id);
        }
        //    [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateTask(Guid id, [FromBody] TaskRequest request)
        {
            var result = await service.UpdateTask(id, request);
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
        //       [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResultModel>> DeleteTask(Guid id)
        {
            var result = await service.DeleteTask(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
