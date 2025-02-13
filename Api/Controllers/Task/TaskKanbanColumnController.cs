using Api.Attributes;
using Aplication.Services.Task;
using Core.ConfigurationProp;
using Core.DTO.TaskDTO;
using Core.Enums;
using Core.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Task
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskKanbanColumnsController : ControllerBase
    {
        private readonly ITaskKanbanColumnService service;

        public TaskKanbanColumnsController(ITaskKanbanColumnService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet("user")]
        [RequirePermissions(Permission.Read)]
        public async Task<ActionResult<List<KanbanColumnDto>>> GetKanbanColumn()
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var columns = await service.GetColumns(userIdGuid);
            if(columns == null)
            {
                return BadRequest();
            }
            var response = columns.Select(r => new KanbanColumnDto(r.id, r.name, r.position));
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult<ResultModelObject<KanbanColumnDto>>> CreateKanbanColumn([FromBody] KanbanColumnCreateDto request)
        {
            try 
            {
                var userId = User.FindFirst(CustomClaims.UserId)?.Value;
                var userIdGuid = new Guid(userId);
                var prop = new KanbanColumnDto(Guid.NewGuid(), request.name, request.position);
                var columns = await service.CreateColumn(prop, userIdGuid);
                var response = columns.Data;
                return Ok(response);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) 
            {
                return Conflict(new
                {
                    Status = 409,
                    Error = "Conflict",
                    Message = "The column 'position' is unique."
                });
            }                 
        }

        [HttpPut]
        [Authorize]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult> UpdateKanbanColumn([FromBody] KanbanColumnDto request)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var result = await service.UpdateColumn(userIdGuid, request);
            if (result == null)
            {
                return NotFound();
            }
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult> DeleteUserKanbanTask(Guid id)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var response = await service.DeleteColumn(id, userIdGuid);
            if (response == null || !response.Success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
