using Core.DTO.UserDTO;
using Core.Enums;

namespace Aplication.Services
{
    public interface IUserGetService
    {
        Task<UserPasswordDto?> GetUser(Guid id);
        Task<List<UserPasswordDto>> GetUsers();
        Task<UserPasswordDto?> GetUserByEmailOrLogin(string login);
        Task<HashSet<Permission>> GetPermissions(Guid userId);
    }
}