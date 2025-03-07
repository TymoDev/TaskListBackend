using Core.DTO.UserDTO;
using Core.ResultModels;
using Microsoft.AspNetCore.Http;

namespace Aplication.Services.User
{
    public interface IUserUpdateService
    {
        Task<ResultModel?> DeleteUser(Guid id);
        Task<ResultModel?> UpdateUser(UserIdDto request);
        Task<ResultModelObject<string>?> UpdateUserProfileImage(Guid userId, IFormFile file);
        Task<ResultModel?> UpdateUserPassword(ResetPasswordDto request);
    }
}