using Core.DTO.UserDTO;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserAuthService
    {
        Task<LoginResultModel> Login(LoginUserDto request);
        Task<LoginResultModel> Register(Guid id,Guid profileId, RegisterUserWithProfileDto request);
        Task<ResultModel> Logout();
    }
}