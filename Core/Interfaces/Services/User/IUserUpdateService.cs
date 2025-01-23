using Core.DTO.UserDTO;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserUpdateService
    {
        Task<ResultModel?> DeleteUser(Guid id);
        Task<ResultModel?> UpdateUser(UserIdDto request);
        Task<ResultModel?> UpdateUserPassword(ResetPasswordDto request);
    }
}