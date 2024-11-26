using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserAuthService
    {
        Task<LoginResultModel> Login(UserRequest request);
        Task<LoginResultModel> Register(Guid id,UserRequest request);
    }
}