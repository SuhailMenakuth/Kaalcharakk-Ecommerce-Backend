namespace Kaalcharakk.Helpers.CloudinaryHelper
{
    public interface ICloudinaryHelper
    {
       Task<string> UploadProductImageAsyn(IFormFile file);
    }
}
