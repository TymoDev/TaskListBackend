using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("auth/[controller]")]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService auth;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAuthController(IUserAuthService auth, IHttpContextAccessor httpContextAccessor)
        {
            this.auth = auth;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            //Registration user
            Guid id = Guid.NewGuid();
            var httpContext = httpContextAccessor.HttpContext;
            var result = await auth.Register(id,request);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            var token = result.Token;

            //Save token in cookies
            httpContext.Response.Cookies.Append("tasty-cookies", token);
            return Ok(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequest request )
        {
            //Create token,Check e-mail and password
            var httpContext = httpContextAccessor.HttpContext;
            var tokenResultModel = await auth.Login(new UserRequest(request.Username,request.Email,request.Password));
            if (!tokenResultModel.Success)
            {
                return BadRequest(tokenResultModel.ErrorMessage);
            }
            var token = tokenResultModel.Token;

            //Save token in cookies
            httpContext.Response.Cookies.Append("tasty-cookies",token);
            return Ok(request.Email);
        }


    }
}
