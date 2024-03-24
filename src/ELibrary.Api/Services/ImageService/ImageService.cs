namespace ELibrary.src.ELibrary.Api.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly string _imageStorePath;
        private readonly string _defaultAvatar;
        public ImageService(IConfiguration configuration)
        {
            _imageStorePath = configuration.GetValue<string>("ImagePath");
            _defaultAvatar = configuration.GetValue<string>("DefaultAvatar");
        }
        public async Task<string> AddImage(IFormFile imagePath)
        {
            string ext = Path.GetExtension(imagePath.FileName);
            string newName = Guid.NewGuid().ToString() + ext;
            string newPath = Path.Combine(_imageStorePath, newName);
            FileStream fs = new(newPath, FileMode.Create, FileAccess.Write);
            await imagePath.CopyToAsync(fs);
            fs.Close();
            return newName;
        }

        public void DeleteImage(string imageName)
        {
            string path = Path.Combine(_imageStorePath, imageName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string GetDefaultAvatar()
        {
            return _defaultAvatar;
            //throw new NotImplementedException();
        }

        public async Task<string> GetImage(string imageName)
        {
            string ext = Path.GetExtension(imageName);
            ext = ext.Replace(".", string.Empty);
            string path = Path.Combine(_imageStorePath, imageName);
            byte[] buff = await File.ReadAllBytesAsync(path);
            string base64Str = "data:image/" + ext + ";base64,"
                + Convert.ToBase64String(buff);
            return base64Str;
        }
    }
}
