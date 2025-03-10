using Core.DTO.UserDTO;
using Core.Interfaces.Logging;
using Core.Interfaces.Repositories;
using Core.ResultModels;
using Core.ValidationModels.User;
using Infrastracture.Logic;
using Infrastracture.Photos;
using Microsoft.AspNetCore.Http;
using Persistance.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services.User
{
    public class UserUpdateService : IUserUpdateService
    {
        private readonly IUserRepository repository;
        private readonly IUserProfileRepository profileRepository;
        private readonly IPasswordHasher hasher;
        private readonly IAppLogger logger;
        private readonly ICloudinaryLogic cloudinary;

        public UserUpdateService(IUserRepository repository, IUserProfileRepository profileRepository, IPasswordHasher hasher, IAppLogger logger, ICloudinaryLogic cloudinary)
        {
            this.repository = repository;
            this.profileRepository = profileRepository;
            this.hasher = hasher;
            this.logger = logger;
            this.cloudinary = cloudinary;
        }

        public async Task<ResultModel?> UpdateUser(UserIdDto request)
        {
            logger.Information($"Updating user with ID: {request.id}");

            var validationResult = UsernameModel.Create(request.Username);
            if (!validationResult.Success)
            {
                logger.Warning($"Username validation failed for user ID: {request.id}, Error: {validationResult.ErrorMessage}");
                return ResultModel.Error(validationResult.ErrorMessage);
            }

            var passwordModelResult = UserPasswordModel.Create(request.Password);
            if (!passwordModelResult.Success)
            {
                logger.Warning($"Password validation failed for user ID: {request.id}, Error: {passwordModelResult.ErrorMessage}");
                return ResultModel.Error(passwordModelResult.ErrorMessage);
            }

            var passwordHash = hasher.Generate(request.Password);
            logger.Information($"Generated password hash for user ID: {request.id}");

            var result = await repository.UpdateUser(new UserHashDto(request.id, request.Username, request.Email, passwordHash));
            if (result == null)
            {
                logger.Error($"Failed to update user with ID: {request.id}");
                return null;
            }

            logger.Information($"Successfully updated user with ID: {request.id}");
            return ResultModel.Ok();
        }

        public async Task<ResultModel?> UpdateUserPassword(ResetPasswordDto request)
        {
            logger.Information($"Updating password for user with email: {request.Email}");

            var validationResult = UserPasswordModel.Create(request.Code);
            if (!validationResult.Success)
            {
                logger.Warning($"Password validation failed for email: {request.Email}, Error: {validationResult.ErrorMessage}");
                return ResultModel.Error(validationResult.ErrorMessage);
            }

            var passwordHash = hasher.Generate(request.Code);
            logger.Information($"Generated password hash for email: {request.Email}");

            var result = await repository.UpdateUserPassword(request.Email, passwordHash);
            if (result == null)
            {
                logger.Error($"Failed to update password for email: {request.Email}");
                return null;
            }

            logger.Information($"Successfully updated password for email: {request.Email}");
            return ResultModel.Ok();
        }
        public async Task<ResultModelObject<string>?> UpdateUserProfileImage(Guid userId, IFormFile file)
        {
            logger.Information($"Updating profile image for user with id: {userId}");
            var isProfileExist = await profileRepository.GetUserProfile(userId);
            if (isProfileExist == null)
            {
                logger.Error($"Can not find user with id: {userId}");
                return ResultModelObject<string>.Error($"Can not find user with id: {userId}");
            }

            var imageData = await cloudinary.UploadImage(file);
            if (!imageData.Success)
            {
                logger.Information($"Cloudinary error: {imageData.ErrorMessage}");
                return ResultModelObject<string>.Error($"Cloudinary error: {imageData.ErrorMessage}");
            }
            if(!String.IsNullOrEmpty(isProfileExist.imagePublicId))
            {
                await cloudinary.DeleteImage(isProfileExist.imagePublicId);
            }

            var result = repository.UpdateUserProfileImage(userId, imageData.Data.imageUrl, imageData.Data.imagePublicId);
            return ResultModelObject<string>.Ok(imageData.Data.imageUrl);
        }

        public async Task<ResultModel?> DeleteUser(Guid id)
        {
            logger.Information($"Deleting user with ID: {id}");

            var result = await repository.DeleteUser(id);
            if (result == null)
            {
                logger.Error($"Failed to delete user with ID: {id}");
                return null;
            }

            logger.Information($"Successfully deleted user with ID: {id}");
            return ResultModel.Ok();
        }
    }
}
