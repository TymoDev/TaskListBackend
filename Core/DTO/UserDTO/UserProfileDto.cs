using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO
{
    public record UserProfileDto(Guid Id, string? Description, string? TwitterUrl, string? LinkedInUrl, string? GitHubUrl, string? PersonalWebsiteUrl);
}
