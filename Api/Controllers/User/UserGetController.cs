using Aplication.Services;
using Core.DTO.UserDTO.Responce;
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
        public async Task<ActionResult<List<UserResponce>>> GetUsers()
        {
            var users = await service.GetUsers();
            var responce = users.Select(b => new UserResponce(b.Id, b.Username, b.Email));
            return Ok(responce);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponcePassword>> GetUserById(Guid id)
        {
            var userResponse = await service.GetUser(id);
            if (userResponse == null)
            {
                return NotFound();
            }
            var responce = new UserResponce(userResponse.Id, userResponse.Username, userResponse.Email);
            return Ok(responce);

        }
        [HttpGet("{email}")]
        public async Task<ActionResult<UserResponcePassword>> GetUserByEmail(string email)
        {
            var userResponse = await service.GetUserByEmail(email);
            if (userResponse == null)
            {
                return NotFound();
            }
            var responce = new UserResponce(userResponse.Id, userResponse.Username, userResponse.Email);
            return Ok(responce);

        }
    }
}
