namespace ELibrary.src.ELibrary.Api.Services.ImageService
{
    public interface IImageService
    {
        Task<string> AddImage(IFormFile imagePath);
        void DeleteImage(string imageName);
        Task<string> GetImage(string imageName);
        string GetDefaultAvatar();
    }
}
