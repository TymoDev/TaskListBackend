using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ResultModels;

namespace Infrastracture.Photos
{
    public class CloudinaryLogic : ICloudinaryLogic
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryLogic(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }
        public async Task<ResultModelObject<string>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return ResultModelObject<string>.Error("File not found");

            using var stream = file.OpenReadStream();
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}";
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = $"images/{uniqueFileName}",
                Overwrite = true
            };


            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return ResultModelObject<string>.Error(uploadResult.Error.Message);

            return ResultModelObject<string>.Ok(uploadResult.SecureUrl.ToString());
        }
        public ResultModelObject<string> GetImage(string publicId)
        {
            var url = cloudinary.Api.UrlImgUp.BuildUrl(publicId.Contains(".") ? publicId : $"{publicId}.jpg");
            return ResultModelObject<string>.Ok(url);
        }

    }
}
