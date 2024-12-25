using Core.DTO.UserDTO.Responce;
using Core.Entities;

namespace Core.Interfaces.Providers
{
    public interface IJwtProvider
    {
        string GenerateAuthenticateToken(UserResponcePassword user);
    }
}