using Core.DTO.UserDTO.Responce;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.ResultModels;
using Core.ValidationModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class UserGetService : IUserGetService
    {
        private readonly IUserRepository repository;

        public UserGetService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<List<UserResponcePassword>> GetUsers()
        {
            var users = await repository.GetUsers();
            return users;
        }
        public async Task<UserResponcePassword?> GetUser(Guid id)
        {
            var responce = await repository.GetUserById(id);
            if (responce == null)
            {
                return null;
            }
            return responce;
        }
        public async Task<UserResponcePassword?> GetUserByEmailOrLogin(string login)
        {
            var responce = await repository.GetUserByEmailOrUsername(login);
            if (responce == null)
            {
                return null;
            }
            return responce;
        }
        public async Task<HashSet<Permission>> GetPermissions(Guid userId)
        {
            return await repository.GetUserPermissions(userId);
        }
    }
}
