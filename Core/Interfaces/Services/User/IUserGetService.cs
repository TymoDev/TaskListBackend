using Core.DTO.UserDTO.Responce;

namespace Aplication.Services
{
    public interface IUserGetService
    {
        Task<UserResponcePassword?> GetUser(Guid id);
        Task<List<UserResponcePassword>> GetUsers();
        Task<UserResponcePassword?> GetUserByEmail(string email);
    }
}