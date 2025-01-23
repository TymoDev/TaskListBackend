using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Core.Interfaces.Logging;
using Core.Interfaces.Repositories;
using Core.ResultModels;
using Core.ValidationModels.User;
using Infrastracture.Logic;
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
        private readonly IPasswordHasher hasher;
        private readonly IAppLogger logger;

        public UserUpdateService(IUserRepository repository, IPasswordHasher hasher, IAppLogger logger)
        {
            this.repository = repository;
            this.hasher = hasher;
            this.logger = logger;
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

            var validationResult = UserPasswordModel.Create(request.Password);
            if (!validationResult.Success)
            {
                logger.Warning($"Password validation failed for email: {request.Email}, Error: {validationResult.ErrorMessage}");
                return ResultModel.Error(validationResult.ErrorMessage);
            }

            var passwordHash = hasher.Generate(request.Password);
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
