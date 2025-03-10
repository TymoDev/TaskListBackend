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
using Core.Entities;
using Core.DTO.UserDTO;

namespace Infrastracture.Photos
{
    public class CloudinaryLogic : ICloudinaryLogic
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryLogic(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }
        public async Task<ResultModelObject<ProfileImageDto>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return ResultModelObject<ProfileImageDto>.Error("File not found");

            using var stream = file.OpenReadStream();
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}";
            var publicId = $"images/{uniqueFileName}";
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Overwrite = true
            };


            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return ResultModelObject<ProfileImageDto>.Error(uploadResult.Error.Message);

            return ResultModelObject<ProfileImageDto>.Ok(new ProfileImageDto(uploadResult.SecureUrl.ToString(), publicId));
        }
        public ResultModelObject<string> GetImage(string publicId)
        {
            var url = cloudinary.Api.UrlImgUp.BuildUrl(publicId.Contains(".") ? publicId : $"{publicId}.jpg");
            return ResultModelObject<string>.Ok(url);
        }

        public async Task<ResultModel> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await cloudinary.DestroyAsync(deleteParams);

            if (deleteResult.Result == "ok")
                return ResultModel.Ok();

            return ResultModel.Error("Failed to delete image");
        }
    }
}