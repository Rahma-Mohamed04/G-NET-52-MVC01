using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };
        private readonly IWebHostEnvironment env;

        public AttachmentService(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public bool Delete(string fileName, string folderName)
        {
            var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);
            try
            {
                if (!File.Exists(fullPath)) return false;

                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return null;

            var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);
            if (!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(fullPath).ToLower();

            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream" // => Binary
            };

            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream == null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;

            if (fileStream.Length > MaxFileSize) return null;

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension)) return null;

            var UploadFolder = Path.Combine(env.ContentRootPath, folderName);
            Directory.CreateDirectory(UploadFolder);
            
            var StoredFileName = $"{Guid.NewGuid()} {fileName}";
            var filePath = Path.Combine(UploadFolder, StoredFileName);

            try
            {
                using var FS = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(FS);
                return StoredFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
