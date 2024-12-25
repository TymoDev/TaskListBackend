using Aplication.Services.User;
using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
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
        public async Task<IActionResult> ResetPasswordCodeCreate(string email)
        {
            var result =await service.ResetPasswordNotify(email);
            return Ok();
        }
        [HttpPost("reset/password/code")]
        public async Task<IActionResult> ResetPasswordCodeVerify(ResetPasswordVerifyRequest request)
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
