using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using Microsoft.Extensions.Options;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            _cloudinary = new Cloudinary(new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            ));
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "twist_and_solve/profile_pictures"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}
