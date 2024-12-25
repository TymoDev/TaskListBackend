﻿using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAuthController(IUserAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            this.auth = authService;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            Guid id = Guid.NewGuid();
            var httpContext = httpContextAccessor.HttpContext;
            try
            {
                var result = await auth.Register(id, request);

                if (!result.Success)
                {
                    return BadRequest(result.ErrorMessage);
                }

                var token = result.Token;

                // Збереження токена в cookies
                httpContext.Response.Cookies.Append("tasty-cookies", token);
                return Ok(id);
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
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            //Create token,Check e-mail and password
            var httpContext = httpContextAccessor.HttpContext;
            var tokenResultModel = await auth.Login(new LoginUserRequest(request.Login,request.Password));
            if (!tokenResultModel.Success)
            {
                return BadRequest(tokenResultModel.ErrorMessage);
            }
            var token = tokenResultModel.Token;

            //Save token in cookies
            httpContext.Response.Cookies.Append("tasty-cookies",token);
            return Ok(request.Login);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TryAuthUser()
        {
            return Ok();
        }
    }
}
