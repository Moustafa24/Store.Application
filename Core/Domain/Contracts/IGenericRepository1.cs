
namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : Models.BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id); 
    }
}