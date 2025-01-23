using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO
{
    public record RegisterUserWithProfileDto(string Login, string Email, string Password,string Username,string? Gender,string? Location, string? Description, string? TwitterUrl, string? LinkedInUrl, string? GitHubUrl, string? PersonalWebsiteUrl);
}
