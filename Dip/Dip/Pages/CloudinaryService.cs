using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dip.Pages
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new CloudinaryDotNet.Account(
                "dphg08qh2",   // Cloud Name
                "188338944798996", // API Key
                "QOeOUhfRzSpAIojur5KTSGfyWEk" // API Secret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Tags = "menu_items",
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }


        public async Task<List<string>> GetImagesAsync()
        {
            var listParams = new ListResourcesParams()
            {
                MaxResults = 10
            };

            var results = await _cloudinary.ListResourcesAsync(listParams);
            return results.Resources.Select(r => r.SecureUrl.ToString()).ToList();
        }
    }
}