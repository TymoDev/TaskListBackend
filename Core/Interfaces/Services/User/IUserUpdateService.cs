using Core.DTO.UserDTO;
using Core.DTO.UserDTO.Request;
using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserUpdateService
    {
        Task<ResultModel?> DeleteUser(Guid id);
        Task<ResultModel?> UpdateUser(UserRequestId request);
        Task<ResultModel?> UpdateUserPassword(ResetPasswordRequest request);
    }
}