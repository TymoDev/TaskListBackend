﻿using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Responce;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public  Task<List<UserResponcePassword>> GetUsers();
        public  Task<UserResponcePassword?> GetUserById(Guid id);
        public Task<UserResponcePassword?> GetUserByEmailOrUsername(string login);
        public  Task<Guid?> UpdateUser(UserRequestHash user);
        public  Task<Guid?> CreateUser(UserRequestHash user);
        public  Task<Guid?> DeleteUser(Guid id);
        
    }
}
