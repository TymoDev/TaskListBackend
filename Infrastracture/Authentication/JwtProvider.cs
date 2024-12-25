using Core.DTO.UserDTO.Responce;
using Core.Entities;
using Core.Interfaces.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        //Creating token
        private readonly JwtOptions options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
        }

        public string GenerateAuthenticateToken(UserResponcePassword user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];
            var signingCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                 SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 claims: claims,
                 signingCredentials: signingCredentials,
                 expires: DateTime.UtcNow.AddHours(options.ExpiresHours)
                );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
