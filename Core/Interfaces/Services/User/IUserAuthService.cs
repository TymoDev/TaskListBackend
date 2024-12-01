using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserAuthService
    {
        Task<LoginResultModel> Login(LoginUserRequest request);
        Task<LoginResultModel> Register(Guid id,RegisterUserRequest request);
    }
}