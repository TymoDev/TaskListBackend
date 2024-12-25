using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Responce;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.Repositories.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Repositories
{
    public class UserRepository : CrudAbstractions<UserEntity>, IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context) : base(context, context.Users)
        {
            this.context = context;
        }
        public async Task<List<UserResponcePassword>> GetUsers()
        {
            return await Get(u => new UserResponcePassword(u.Id,u.UserName,u.Email,u.PasswordHash));
        }
        public async Task<UserResponcePassword?> GetUserById(Guid id)
        {
            return await GetById(id,u => new UserResponcePassword(u.Id, u.UserName, u.Email,u.PasswordHash));
        }
        public async Task<UserResponcePassword?> GetUserByEmailOrUsername(string login)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == login || u.UserName == login);
            if (user == null)
            {
                return null;
            }
            return new UserResponcePassword(user.Id, user.UserName, user.Email, user.PasswordHash);
        }

        public async Task<Guid?> UpdateUser(UserRequestHash userData)
        {
            var user = GetById(userData.Id, u => u);
            if(user == null)
            {
                return null;
            }
            return await Update(userData.Id, OldData =>
            {
                OldData.UserName = userData.UserName;
                OldData.PasswordHash = userData.PasswordHash;
                OldData.Email = userData.Email;
            });



        }
        public async Task<Guid?> CreateUser(UserRequestHash userData)
        {
            var userEntity = new UserEntity
            {
                Id = userData.Id,
                UserName = userData.UserName,
                Email = userData.Email,
                PasswordHash = userData.PasswordHash,
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

        
    }
}
