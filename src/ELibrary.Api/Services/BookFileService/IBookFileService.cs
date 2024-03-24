namespace ELibrary.src.ELibrary.Api.Services.BookFileService
{
    public interface IBookFileService
    {
        Task<string> AddBookFile(IFormFile filePath);
        void DeleteBookFile(string fileName);
        string GetBookFilePath(string fileName);
    }
}
