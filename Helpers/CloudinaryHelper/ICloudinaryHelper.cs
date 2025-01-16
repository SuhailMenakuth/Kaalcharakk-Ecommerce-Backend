namespace Kaalcharakk.Helpers.CloudinaryHelper
{
    public interface ICloudinaryHelper
    {
       Task<string> UploadProductImageAsyn(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);
        string ExtractPublicIdFromUrl(string imageUrl);


    }
}
