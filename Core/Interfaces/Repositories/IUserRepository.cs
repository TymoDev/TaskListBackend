using Core.DTO.UserDTO;

using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public  Task<List<UserPasswordDto>> GetUsers();
        public  Task<UserPasswordDto?> GetUserById(Guid id);
        public Task<UserPasswordDto?> GetUserByEmailOrUsername(string login);
        public  Task<Guid?> UpdateUser(UserHashDto user);
        Task<string?> UpdateUserPassword(string email, string password);
        public  Task<Guid?> CreateUser(Guid userId,Guid profileId, RegisterUserWithProfileDto data);
        public  Task<Guid?> DeleteUser(Guid id);
        Task<HashSet<Permission>> GetUserPermissions(Guid userId);


    }
}
