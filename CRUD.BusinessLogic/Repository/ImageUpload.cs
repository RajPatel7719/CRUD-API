using CRUD.BusinessLogic.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CRUD.BusinessLogic.Repository
{
    public class ImageUpload : IImageUpload
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        public ImageUpload(IConfiguration configuration, IUserRepositoryAsync userRepositoryAsync)
        {
            _configuration = configuration;
            _userRepositoryAsync = userRepositoryAsync;
        }
        public async Task<string> SaveImage(IFormFile imageFile, string email)
        {
            string imageName = "";
            if (imageFile != null)
            {
                try
                {
                    if (imageFile.Length > 0)
                    {

                        imageName = GetUniqueFileName(imageFile.FileName, email);

                        var imagePath = Path.Combine(_configuration["ImageFilePath"], imageName);

                        if (!Directory.Exists(_configuration["ImageFilePath"]))
                        {
                            Directory.CreateDirectory(_configuration["ImageFilePath"]);
                        }

                        if (!File.Exists(imagePath))
                        {
                            using (FileStream fileStream = new(imagePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            File.Delete(imagePath);
                            using (FileStream fileStream = new(imagePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("File Copy Failed", ex);
                }
            }
            return imageName;
        }

        public async Task<FileStream> GetImage(string ImageName)
        {
            FileStream? imageFileStream = null;
            if (!string.IsNullOrEmpty(ImageName))
            {
                var path = Path.Combine(_configuration["ImageFilePath"], "images", $"{ImageName}");
                imageFileStream = File.OpenRead(path);
                return imageFileStream;
            }
            return imageFileStream;
        }

        public async Task DeleteImage(string ImageName)
        {
            if (!string.IsNullOrEmpty(ImageName))
            {
                var imagePath = Path.Combine(_configuration["ImageFilePath"], "image", ImageName);
                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }
        }

        public string GetUniqueFileName(string fileName, string email)
        {
            string imageName = "";
            var Result = _userRepositoryAsync.GetUserByEmailAsync(email).Result;

            imageName = Result.Id + Path.GetExtension(fileName);
            return imageName;
        }
    }
}
