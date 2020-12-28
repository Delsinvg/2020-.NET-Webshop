using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using project.models.Images;
using project.shared.Settings;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Helpers
{
    public static class ImageHelper
    {
        private static FileUploadSettings _fileUploadSettings;

        public static void Configure(IOptions<FileUploadSettings> fileUploadSettings)
        {
            _fileUploadSettings = fileUploadSettings.Value;
        }

        public static string GetImage(byte[] rawImageData, string bestandstype, string type)
        {
            if (rawImageData != null)
            {
                string dataBase64 = Convert.ToBase64String(rawImageData);
                string imageData = string.Format("data:{0};base64,{1}", bestandstype, dataBase64);
                return imageData;
            }

            if (type.Equals("product"))
            {
                return "/img/product.jpg";
            } else
            {
                return "/img/user.jpg";
            }
        }

        public static async Task<bool> ValidateImage(IFormFile afbeelding, ModelStateDictionary modelState, string modelProperty)
        {
            bool valid = false;
            string extension = Path.GetExtension(afbeelding.FileName).ToLowerInvariant();
            string contentType = afbeelding.ContentType.ToLowerInvariant();

            if (!_fileUploadSettings.FileExtensions.Contains(extension))
            {
                modelState.AddModelError(modelProperty, $"De extensie {extension} is niet toegelaten");
            }
            else if (!_fileUploadSettings.FileContentTypes.Contains(contentType))
            {
                modelState.AddModelError(modelProperty, $"Het bestandstype {contentType} is niet toegelaten");
            }
            else if (afbeelding.Length > int.Parse(_fileUploadSettings.FileSizeLimit))
            {
                modelState.AddModelError(modelProperty, "Het bestand is groter dan " + int.Parse(_fileUploadSettings.FileSizeLimit) / 1048576 + "MB");
            }
            else
            {
                using var memoryStream = new MemoryStream();
                await afbeelding.CopyToAsync(memoryStream);
                var info = new MagickImageInfo(memoryStream.ToArray());

                if (info.Width > 300)
                {
                    modelState.AddModelError(modelProperty, "De afbeelding mag maximaal 300px breed zijn");
                }
                else
                {
                    valid = true;
                }
            }

            return valid;
        }

        public static async Task<ImageModel> SetAfbeelding(IFormFile file, string description)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return new ImageModel
            {
                Name = Guid.NewGuid().ToString(),
                FileType = file.ContentType,
                Extension = extension,
                Data = memoryStream.ToArray(),
                Description = description
            };
        }
    }
}
