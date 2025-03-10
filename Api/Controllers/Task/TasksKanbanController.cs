﻿using Api.Attributes;
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
    public class TasksKanbanController : ControllerBase
    {
        private readonly ITaskKanbanService service;

        public TasksKanbanController(ITaskKanbanService service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet("user")]
        [RequirePermissions(Permission.Read)]
        public async Task<ActionResult<List<TaskKanbanGetDto>>> GetUserKanbanTasks()
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var tasks = await service.GetUserTasks(userIdGuid);
            var response = tasks.Select(r => new TaskKanbanGetDto(r.taskId, r.taskName, r.columnId,r.order));
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult<ResultModelObject<TaskKanbanOrderDto>>> CreateUserKanbanTask([FromBody] TaskKanbanCreateDto request)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var prop = new TaskKanbanDto(Guid.NewGuid(),request.taskName,request.columnId,userIdGuid);
            var tasks = await service.CreateUserTasks(prop,userIdGuid);
            var response = tasks.Data;
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult<ResultModelObject<TaskKanbanOrderDto>>> UpdateUserKanbanTask([FromBody] TaskKanbanUpdateDto request)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var result = await service.UpdateTask(userIdGuid,request);
            if (result == null)
            {
                return NotFound();
            }
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult> DeleteUserKanbanTask(Guid id)
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
