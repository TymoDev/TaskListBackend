using Core.DTO.UserDTO;
using Core.Entities;

namespace Core.Interfaces.Providers
{
    public interface IJwtProvider
    {
        string GenerateAuthenticateToken(UserPasswordDto user);
    }
}