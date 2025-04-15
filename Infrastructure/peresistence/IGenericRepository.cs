namespace peresistence
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : Domain.Models.BaseEntity<TKey>
    {
    }
}