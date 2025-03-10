using Core.DTO.UserDTO;
using Core.ResultModels;
using Microsoft.AspNetCore.Http;
namespace Infrastracture.Photos
{
    public interface ICloudinaryLogic
    {
        ResultModelObject<string> GetImage(string publicId);
        Task<ResultModelObject<ProfileImageDto>> UploadImage(IFormFile file);
        Task<ResultModel> DeleteImage(string publicId);
    }
}