using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
    }
}
