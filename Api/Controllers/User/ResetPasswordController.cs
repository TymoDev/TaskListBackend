using Aplication.Services.User;
using Core.DTO.UserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class ResetPasswordController : Controller
    {
        private readonly IUserResetPasswordService service;

        public ResetPasswordController(IUserResetPasswordService service)
        {
            this.service = service;
        }
        [HttpPost("reset/password")]
        public async Task<IActionResult> ResetPasswordCodeCreate( ResetPasswordDto request)
        {
            var result = await service.ResetPasswordNotify(request.Email);
            return Ok();
        }
        [HttpPost("reset/password/code")]
        public async Task<IActionResult> ResetPasswordCodeVerify(ResetPasswordVerifyDto request)
        {
            var result = await service.ResetPasswordVerify(request.Email,request.Code);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
    }
}
