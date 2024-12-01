using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Core.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("update/[controller]")]
    public class UserUpdateController : ControllerBase
    {
        private readonly IUserUpdateService service;

        public UserUpdateController(IUserUpdateService service)
        {
            this.service = service;
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateUser(Guid id, [FromBody] RegisterUserRequest request)
        {
            ResultModel result;
            try
            {
                result = await service.UpdateUser(new UserRequestId(id, request.Username, request.Email, request.Password));
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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResultModel>> DeleteUser(Guid id)
        {
            var responce = await service.DeleteUser(id);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }
    }
}
