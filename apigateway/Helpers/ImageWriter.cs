using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace apigateway
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file, string imageFolderPath);
    }

    public class ImageWriter : IImageWriter
    {
        public async Task<string> UploadImage(IFormFile file, string imageFolderPath)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFile(file, imageFolderPath);
            }

            return "Invalid image file";
        }

        /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return ImageWriterHelper.GetImageFormat(fileBytes) != ImageWriterHelper.ImageFormat.Unknown;
        }

        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageFolderPath"></param>
        /// <returns></returns>
        private static async Task<string> WriteFile(IFormFile file, string imageFolderPath)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid() + extension; //Create a new Name
                //for the file due to security reasons.
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), imageFolderPath));
                var path = Path.Combine(Directory.GetCurrentDirectory(), imageFolderPath, fileName);

                await using var bits = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(bits);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }
    }
}