using Core.DTO.UserDTO.Responce;
using Core.Entities;

namespace Infrastracture.Logic
{
    public interface IJwtProvider
    {
        string GenerateToken(UserResponcePassword user);
    }
}