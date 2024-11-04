using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
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

        public UserUpdateService(IUserRepository repository,IPasswordHasher hasher)
        {
            this.repository = repository;
            this.hasher = hasher;
        }
        public async Task<ResultModel?> UpdateUser(UserRequestId request)
        {
            var userModelResult = UsernameModel.Create(request.Username);
            if (!userModelResult.Success)
            {

                return ResultModel.Error(userModelResult.ErrorMessage);
            }
            var passwordModelResult = UserPasswordModel.Create(request.Password);
            if (!passwordModelResult.Success)
            {
                return ResultModel.Error(passwordModelResult.ErrorMessage);
            }
            var passwordHash = hasher.Generate(request.Password); 
            var result = await repository.UpdateUser(new UserRequestHash(request.id, request.Username, request.Email, passwordHash));
            if (result == null)
            {
                return null;
            }
            return ResultModel.Ok();
        }
        public async Task<ResultModel?> DeleteUser(Guid id)
        {
            var result = await repository.DeleteUser(id);
            if (result == null)
            {
                return null;
            }
            return ResultModel.Ok();
        }
    }
}
