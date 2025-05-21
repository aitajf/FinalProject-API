using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", folder);
            if (!Directory.Exists(uploadRoot))
            {
                Directory.CreateDirectory(uploadRoot);
            }
            string filePath = Path.Combine(uploadRoot, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string fileUrl = $"/Uploads/{folder}/{fileName}";
            return fileUrl;
        }

        public void Delete(string fileName, string folder)
        {

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("File name or folder cannot be empty.");
            }

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", folder);
            string filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found at path: {filePath}");
                 File.Delete(filePath);          
        }
    }
}
