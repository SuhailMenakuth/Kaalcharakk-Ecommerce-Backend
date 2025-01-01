
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Kaalcharakk.Helpers.CloudinaryHelper
{
    public class CloudinaryHelper : ICloudinaryHelper
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper(IConfiguration configuration)
        {
            var cloudName = configuration["cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadProductImageAsyn(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {

                throw new ArgumentException("File cannot be null or empty", nameof(file));
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "KaalcharakkImageStor", 
                Transformation = new Transformation().Quality("auto").FetchFormat("auto") 
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if(uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();

        }
      
    }
}
