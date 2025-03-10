using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserProfileEntity
{
    [Key] 
    [ForeignKey("User")]
    public required Guid UserId { get; set; }
    public required string Username { get; set; }

    [ForeignKey("ProfileImage")]
    public Guid? ProfileImageId { get; set; }
    public string? Gender { get; set; }
    public string? Birthday { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? TwitterUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PersonalWebsiteUrl { get; set; }
    public ProfileImagesEntity? ProfileImage { get; set; }
    public UserEntity User { get; set; } = null!; 
}
