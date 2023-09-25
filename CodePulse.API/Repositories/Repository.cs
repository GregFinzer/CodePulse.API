using CodePulse.API.Data;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            entity.GetType().GetProperty("Id")?.SetValue(entity, Guid.NewGuid());
            var dbSet = _dbContext.Set<TEntity>();
            await dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            var primaryKeyValue = GetPrimaryKeyValue(_dbContext, entity);
            var existingEntity = await dbSet.FindAsync(primaryKeyValue);

            if (existingEntity == null)
            {
                return null;
            }

            dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(object id)
        {
            var dbSet = _dbContext.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private object GetPrimaryKeyValue<TEntity>(DbContext context, TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // Get the entity type of TEntity from the DbContext
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            // Get the primary key for the entity
            var primaryKey = entityType.FindPrimaryKey();

            // Return the value of the property used as the primary key
            var keyProperty = primaryKey?.Properties.FirstOrDefault();
            return keyProperty?.GetGetter().GetClrValue(entity);
        }
    }
}
