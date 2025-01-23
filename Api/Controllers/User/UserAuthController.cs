using Aplication.Services.User;
using Core.ConfigurationProp;
using Core.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService auth;
        private readonly ICookieHandler cookieHandler;

        public UserAuthController(IUserAuthService authService, ICookieHandler cookieHandler)
        {
            this.auth = authService;
            this.cookieHandler = cookieHandler;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserWithProfileDto request)
        {
            Guid userId = Guid.NewGuid();
            Guid profileId = Guid.NewGuid();
            try
            {
                var result = await auth.Register(userId, profileId, request);

                if (!result.Success)
                {
                    return BadRequest(result.ErrorMessage);
                }

                var token = result.Token;

                cookieHandler.SetCookie(CookieProps.CookieName, token, 7);
                return Ok(new UserAndProfileIdDto(userId,profileId));
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
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto request)
        {
            var tokenResultModel = await auth.Login(new LoginUserDto(request.Login,request.Password));
            if (!tokenResultModel.Success)
            {
                return BadRequest(tokenResultModel.ErrorMessage);
            }
            var token = tokenResultModel.Token;
            cookieHandler.SetCookie(CookieProps.CookieName, token, 7);
            return Ok(request.Login);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await auth.Logout();
            cookieHandler.RemoveCookie(CookieProps.CookieName);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult TryAuthUser()
        {
            return Ok();
        }
    }
}
