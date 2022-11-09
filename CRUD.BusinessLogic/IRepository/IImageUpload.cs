using Microsoft.AspNetCore.Http;

namespace CRUD.BusinessLogic.IRepository
{
    public interface IImageUpload
    {
        Task DeleteImage(string ImageName);
        Task<FileStream> GetImage(string ImageName);
        Task<string> SaveImage(IFormFile imageFile, string email);
    }
}