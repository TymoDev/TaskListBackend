using Core.ResultModels;

namespace Aplication.Services.User
{
    public interface IUserResetPasswordService
    {
        Task<ResultModel> ResetPasswordNotify(string email);
        Task<ResultModel> ResetPasswordVerify(string email, int code);
    }
}