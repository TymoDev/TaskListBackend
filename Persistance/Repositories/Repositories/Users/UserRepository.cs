using Core.DTO.UserDTO;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.Repositories.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Repositories.Users
{
    public class UserRepository : CrudAbstractions<UserEntity>, IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context) : base(context, context.Users)
        {
            this.context = context;
        }
        public async Task<List<UserPasswordDto>> GetUsers()
        {
            return await Get(u => new UserPasswordDto(u.Id, u.Login, u.Email, u.PasswordHash));
        }
        public async Task<UserPasswordDto?> GetUserById(Guid id)
        {
            return await GetById(id, u => new UserPasswordDto(u.Id, u.Login, u.Email, u.PasswordHash));
        }
        public async Task<UserPasswordDto?> GetUserByEmailOrUsername(string login)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == login || u.Login == login);
            if (user == null)
            {
                return null;
            }
            return new UserPasswordDto(user.Id, user.Login, user.Email, user.PasswordHash);
        }

        public async Task<Guid?> UpdateUser(UserHashDto userData)
        {
            var user = GetById(userData.Id, u => u);
            if (user == null)
            {
                return null;
            }
            return await Update(userData.Id, OldData =>
            {
                OldData.Login = userData.UserName;
                OldData.PasswordHash = userData.PasswordHash;
                OldData.Email = userData.Email;
            });
        }
        public async Task<string?> UpdateUserPassword(string email, string password)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            user.PasswordHash = password;
            await context.SaveChangesAsync();
            return user.Email;
        }
        public async Task<string?> UpdateUserProfileImage(Guid userId, string imageUrl, string imagePublicId)
        {
            var userProfile = await context.UsersProfiles
              .Include(u => u.ProfileImage)
              .SingleOrDefaultAsync(u => u.UserId == userId);
            if (userProfile == null)
            {
                return null;
            }
            if(userProfile.ProfileImage == null)
            {
                var newProfileImage = new ProfileImagesEntity
                {
                    Id = Guid.NewGuid(),
                    ProfileImageUrl = imageUrl,
                    ImagePublicId = imagePublicId
                };

                context.ProfileImages.Add(newProfileImage);
                userProfile.ProfileImageId = newProfileImage.Id;
                userProfile.ProfileImage = newProfileImage;
            }
            else
            {
                userProfile.ProfileImage.ProfileImageUrl = imageUrl;
                userProfile.ProfileImage.ImagePublicId = imagePublicId;
            }

            await context.SaveChangesAsync();
            return userProfile.ProfileImage.ProfileImageUrl;
        }

        public async Task<Guid?> CreateUser(Guid userId, Guid profileId, RegisterUserWithProfileDto data)
        {
            var roleEntity = await context.Roles
            .SingleOrDefaultAsync(r => r.Id == (int)Role.User)
            ?? throw new InvalidOperationException();

            var userEntity = new UserEntity
            {
                Id = userId,
                Login = data.login,
                Email = data.email,
                PasswordHash = data.password,
                Roles = [roleEntity],
                Profile = new UserProfileEntity
                {
                    Username = data.username,
                    Gender = data.gender,
                    Birthday = data.birthday,
                    Location = data.location,
                    Description = data.description,
                    TwitterUrl = data.twitterUrl,
                    LinkedInUrl = data.linkedInUrl,
                    GitHubUrl = data.gitHubUrl,
                    PersonalWebsiteUrl = data.personalWebsiteUrl,
                    UserId = userId
                }
            };
            return await Create(userEntity);
        }

        public async Task<Guid?> DeleteUser(Guid id)
        {
            var user = await GetById(id, u => u);
            if (user == null)
            {
                return null;
            }
            return await Delete(id);
        }
        public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
        {
            var roles = await context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.Id == userId)
                .Select(u => u.Roles)
                .ToListAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (Permission)p.Id)
                .ToHashSet();
        }
    }
}
