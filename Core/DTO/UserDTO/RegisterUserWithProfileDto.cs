using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO
{
    public record RegisterUserWithProfileDto(string login, string email, string password,string username,string? gender,string? birthday, string? location, string? description, string? twitterUrl, string? linkedInUrl, string? gitHubUrl, string? personalWebsiteUrl);
}
