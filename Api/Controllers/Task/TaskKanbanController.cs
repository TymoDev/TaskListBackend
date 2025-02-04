using Api.Attributes;
using Aplication.Services.Task;
using Core.ConfigurationProp;
using Core.DTO.TaskDTO;
using Core.Enums;
using Core.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Repositories.Repositories;
using Sprache;

namespace Api.Controllers.Task
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskKanbanController : ControllerBase
    {
        private readonly ITaskKanbanService service;

        public TaskKanbanController(ITaskKanbanService service)
        {
            this.service = service;
        }
        [Authorize]
        [HttpGet("user")]
        [RequirePermissions(Permission.Read)]
        public async Task<ActionResult<List<TaskKanbanOrderDto>>> GetUserKanbanTasks()
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var tasks = await service.GetUserTasks(userIdGuid);
            var response = tasks.Select(r => new TaskKanbanOrderDto(r.taskId, r.taskName, r.column,r.order,r.userId));
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult<ResultModelObject<TaskKanbanOrderDto>>> CreateUserKanbanTask([FromBody] TaskKanbanDto request)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var tasks = await service.CreateUserTasks(request,userIdGuid);
            var response = tasks.Data;
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult<List<TaskKanbanOrderDto>>> DeleteUserKanbanTask(Guid id)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var response = await service.DeleteUserTasks(id,userIdGuid);
            if (response == null || !response.Success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
