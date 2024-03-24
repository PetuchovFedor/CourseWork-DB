namespace ELibrary.src.ELibrary.Api.Services.BookFileService
{
    public class BookFileService : IBookFileService
    {
        private readonly string _bookFileStorePath;
        public BookFileService(IConfiguration configuration)
        {
            _bookFileStorePath = configuration.GetValue<string>("BookFilePath");
        }
        public async Task<string> AddBookFile(IFormFile filePath)
        {
            string ext = Path.GetExtension(filePath.FileName);
            string newName = Path.GetRandomFileName() + ext;
            string newPath = Path.Combine(_bookFileStorePath, newName);
            FileStream fs = new(newPath, FileMode.Create, FileAccess.Write);
            await filePath.CopyToAsync(fs);
            fs.Close();
            return newName;
            //throw new NotImplementedException();
        }

        public void DeleteBookFile(string fileName)
        {
            string path = Path.Combine(_bookFileStorePath, fileName);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //throw new NotImplementedException();
        }

        public string GetBookFilePath(string fileName)
        {
            return Path.Combine(_bookFileStorePath, fileName);
            //throw new NotImplementedException();
        }
    }
}
