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
        public async Task<ActionResult<Guid>> UpdateUser(Guid id, [FromBody] UserRequest request)
        {
            var result = await service.UpdateUser(new UserRequestId(id,request.Username,request.Email,request.Password));
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
