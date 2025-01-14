using Core.DTO.UserDTO.Responce;
using Core.Enums;

namespace Aplication.Services
{
    public interface IUserGetService
    {
        Task<UserResponcePassword?> GetUser(Guid id);
        Task<List<UserResponcePassword>> GetUsers();
        Task<UserResponcePassword?> GetUserByEmailOrLogin(string login);
        Task<HashSet<Permission>> GetPermissions(Guid userId);
    }
}