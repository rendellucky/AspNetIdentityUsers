namespace HW10.Models.Services
{
    public class ImageStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Image> SaveUploadedFileAsync(IFormFile file)
        {
            var guid = Guid.NewGuid().ToString().ToLower();
            var filename = guid + Path.GetExtension(file.FileName);

            var fullFilename = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);

            using (var localFile = System.IO.File.OpenWrite(fullFilename))
            {
                await file.CopyToAsync(localFile);
            }

            return new Image() { Filename = filename };
        }

        public void RemoveImage(Image image)
        {
            var fullFilename = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", image.Filename);
            File.Delete(fullFilename);
        }
    }
}
