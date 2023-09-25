namespace CodePulse.API.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(object id);
    }
}
