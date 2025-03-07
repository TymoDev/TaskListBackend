using Core.ResultModels;
using Microsoft.AspNetCore.Http;
namespace Infrastracture.Photos
{
    public interface ICloudinaryLogic
    {
        ResultModelObject<string> GetImage(string publicId);
        Task<ResultModelObject<string>> UploadImage(IFormFile file);
    }
}