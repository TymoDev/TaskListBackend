using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserProfileEntity
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public  string? Gender { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? TwitterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PersonalWebsiteUrl { get; set; }

        [ForeignKey("User")]
        public required Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
