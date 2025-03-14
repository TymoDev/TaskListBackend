﻿using Api.Attributes;
using Aplication.Services.User;
using Core.ConfigurationProp;
using Core.DTO.UserDTO;
using Core.Enums;
using Core.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/update/[controller]")]
    public class UserUpdateController : ControllerBase
    {
        private readonly IUserUpdateService service;

        public UserUpdateController(IUserUpdateService service)
        {
            this.service = service;
        }
        [HttpPut("{id:guid}")]
        [Authorize]
        [RequirePermissions(Permission.GetUsers)]
        public async Task<ActionResult<Guid>> UpdateUser(Guid id, [FromBody] UserIdDto request)
        {
            ResultModel result;
            try
            {
                result = await service.UpdateUser(new UserIdDto(id, request.Username, request.Email, request.Password));
            } 
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) 
            {
                return Conflict(new
                {
                    Status = 409,
                    Error = "Conflict",
                    Message = "The email or username already exists. Please choose a different value."
                });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Error = "Internal Server Error",
                    Message = ex.Message
                });
            }
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
        [RequirePermissions(Permission.Write)]
        [HttpPut("password")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUserPassword(ResetPasswordDto request)
        {
            ResultModel result = await service.UpdateUserPassword(request);
            
            if (result == null)
            {
                return NotFound();
            }
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(request.Email);
        }

        [RequirePermissions(Permission.Write)]
        [HttpPut("image")]
        [Authorize]
        public async Task<ActionResult<UpdateProfileImageDto>> UpdateUserProfileImage(IFormFile file)
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var userIdGuid = Guid.Parse(userId);
            ResultModelObject<string> result = await service.UpdateUserProfileImage(userIdGuid, file);

            if (result == null)
            {
                return NotFound();
            }

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            var response = new UpdateProfileImageDto(userIdGuid, result.Data);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        //[RequirePermissions(Permission.GetUsers)]
        public async Task<ActionResult<ResultModel>> DeleteUser(Guid id)
        {
            var responce = await service.DeleteUser(id);
            if (responce == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
