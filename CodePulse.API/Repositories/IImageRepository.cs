using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories
{
    public interface IImageRepository : IRepository<BlogImage>
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
