using CodePulse.API.Data;
using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
