using Api.Attributes;
using Aplication.Services;
using Aplication.Services.User;
using Core.ConfigurationProp;
using Core.DTO.UserDTO;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : Controller
{
    private readonly IUserProfileService service;

    public UserProfileController(IUserProfileService service)
    {
        this.service = service;
    }

    [HttpGet("{id:guid}")]
    [RequirePermissions(Permission.Read)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileById(Guid id)
    {
        var serviceResponse = await service.GetUserProfile(id);
        if (serviceResponse == null)
        {
            return NotFound();
        }
        var response = new UserProfileDto(serviceResponse.Id, serviceResponse.Description, serviceResponse.TwitterUrl, serviceResponse.LinkedInUrl, serviceResponse.GitHubUrl, serviceResponse.PersonalWebsiteUrl);
        return Ok(response);

    }
}