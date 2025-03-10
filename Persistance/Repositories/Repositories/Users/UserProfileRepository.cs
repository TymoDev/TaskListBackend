using Core.DTO.UserDTO;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Repositories.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Repositories.Users
{
    public class UserProfileRepository : CrudAbstractions<UserProfileEntity>, IUserProfileRepository
    {
        private readonly DataContext context;

        public UserProfileRepository(DataContext context) : base(context, context.UsersProfiles)
        {
            this.context = context;
        }
        public async Task<UserProfileDto?> GetUserProfile(Guid userId)
        {
            var userProfileDto = await context.UsersProfiles
              .Include(u => u.ProfileImage)
              .AsNoTracking()
              .Where(u => u.UserId == userId)
              .Select(userProfile => new UserProfileDto(
                  userId,
                  userProfile.Username,
                  userProfile.ProfileImage != null ? userProfile.ProfileImage.ProfileImageUrl : "",
                  userProfile.ProfileImage != null ? userProfile.ProfileImage.ImagePublicId : "",
                  userProfile.Gender,
                  userProfile.Birthday,
                  userProfile.Location,
                  userProfile.Description,
                  userProfile.TwitterUrl,
                  userProfile.LinkedInUrl,
                  userProfile.GitHubUrl,
                  userProfile.PersonalWebsiteUrl
              ))
              .FirstOrDefaultAsync();

            return userProfileDto;
        }
    }
}
