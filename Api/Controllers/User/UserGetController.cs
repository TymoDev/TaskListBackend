using Api.Attributes;
using Aplication.Services;
using Core.ConfigurationProp;
using Core.DTO.UserDTO;
using Core.Enums;
using Core.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGetController : Controller
    {
        private readonly IUserGetService service;

        public UserGetController(IUserGetService service)
        {
            this.service = service;
        }
        [HttpGet]
        [RequirePermissions(Permission.GetUsers)]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await service.GetUsers();
            var responce = users.Select(b => new UserDto(b.Id, b.Username, b.Email));
            return Ok(responce);
        }
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var userId = User.FindFirst(CustomClaims.UserId)?.Value;
            var userIdGuid = Guid.Parse(userId);
            var result = await service.GetUser(userIdGuid);
            if(result == null)
            {
                return Unauthorized();
            }
            var response = new UserDto(result.Id, result.Username, result.Email);
            return Ok(response);
        }


        [HttpGet("{id:guid}")]
        [RequirePermissions(Permission.GetUsers)]
        public async Task<ActionResult<UserPasswordDto>> GetUserById(Guid id)
        {
            var response = await service.GetUser(id);
            if (response == null)
            {
                return NotFound();
            }
            var responce = new UserDto(response.Id, response.Username, response.Email);
            return Ok(responce);

        }
        [HttpGet("{login}")]
        [RequirePermissions(Permission.GetUsers)]
        public async Task<ActionResult<UserPasswordDto>> GetUserByEmailOrLogin(string login)
        {
            var response = await service.GetUserByEmailOrLogin(login);
            if (response == null)
            {
                return NotFound();
            }
            var responce = new UserDto(response.Id, response.Username, response.Email);
            return Ok(responce);
        }
    }
}
