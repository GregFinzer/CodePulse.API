using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories
{
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts
                .Include(bp => bp.Categories).ToListAsync();
        }


        public override async Task<BlogPost> GetByIdAsync(object id)
        {
            return await _dbContext.BlogPosts
                .Include(bp => bp.Categories)
                .FirstOrDefaultAsync(bp => bp.Id == (Guid) id);
        }

        public override async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }

            // Update BlogPost
            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await _dbContext.SaveChangesAsync();

            return blogPost;
        }

        public Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return _dbContext.BlogPosts
                .Include(bp => bp.Categories)
                .FirstOrDefaultAsync(bp => bp.UrlHandle == urlHandle);
        }
    }
}
