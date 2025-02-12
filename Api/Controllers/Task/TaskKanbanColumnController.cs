﻿using Api.Attributes;
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
    public class TaskKanbanColumnController : ControllerBase
    {
        private readonly ITaskKanbanColumnService service;

        public TaskKanbanColumnController(ITaskKanbanColumnService service)
        {
            this.service = service;
        }
        [Authorize]
        [HttpGet("user")]
        [RequirePermissions(Permission.Read)]
        public async Task<ActionResult<List<KanbanColumnDto>>> GetUserKanbanTasks()
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var columns = await service.GetUserColumns(userIdGuid);
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
        public async Task<ActionResult<ResultModelObject<KanbanColumnDto>>> CreateUserKanbanTask([FromBody] KanbanColumnCreateDto request)
        {
            try 
            {
                var userId = User.FindFirst(CustomClaims.UserId)?.Value;
                var userIdGuid = new Guid(userId);
                var prop = new KanbanColumnDto(Guid.NewGuid(), request.name, request.position);
                var columns = await service.CreateUserColumns(prop, userIdGuid);
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

        [Authorize]
        [HttpDelete("{id:guid}")]
        [RequirePermissions(Permission.Write)]
        public async Task<ActionResult> DeleteUserKanbanTask(Guid id)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = new Guid(userId);
            var response = await service.DeleteUserColumns(id, userIdGuid);
            if (response == null || !response.Success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
