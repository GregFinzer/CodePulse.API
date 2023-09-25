using CodePulse.API.Data;
using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories
{
    public class ImageRepository : Repository<BlogImage>, IImageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;

        public ImageRepository(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = contextAccessor;
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using (var stream = new FileStream(localPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _contextAccessor.HttpContext.Request;

            var urlPath =
                $"{request.Scheme}://{request.Host}{request.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            blogImage.Url = urlPath;
            await _dbContext.BlogImages.AddAsync(blogImage);
            await _dbContext.SaveChangesAsync();
            return blogImage;
        }
    }
}
