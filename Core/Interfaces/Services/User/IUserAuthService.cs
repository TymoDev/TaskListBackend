using Core.DTO.UserDTO;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserAuthService
    {
        Task<LoginResultModel> Login(LoginUserDto request);
        Task<LoginResultModel> Register(Guid id,RegisterUserDto request);
        Task<ResultModel> Logout();
    }
}