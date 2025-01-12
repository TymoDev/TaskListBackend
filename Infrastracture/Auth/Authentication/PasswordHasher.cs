using BCrypt.Net;
using Core.DTO.UserDTO;
using Infrastracture.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastracture.Auth.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
