using Aplication.Services.User;
using Core.DTO.UserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

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
        [HttpPost("reset/password/{email}")]
        public async Task<IActionResult> ResetPasswordCodeCreate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email is required" });
            }
            var result = await service.ResetPasswordNotify(email);
            return Accepted();
        }
        [HttpPost("reset/password/code")]
        public async Task<IActionResult> ResetPasswordCodeVerify(ResetPasswordVerifyDto request)
        {
            var result = await service.ResetPasswordVerify(request.Email,request.Code);
            if (!result.Success)
            {
                return Forbid();
            }
            return Ok();
        }
    }
}
