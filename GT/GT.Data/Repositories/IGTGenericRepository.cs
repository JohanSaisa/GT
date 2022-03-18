using GT.Data.Data;
using Microsoft.EntityFrameworkCore.Query;

namespace GT.Data.Repositories
{
	public interface IGTGenericRepository<TEntity>
		where TEntity : class, IGTEntity
	{
		Task<IQueryable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
    Task<TEntity> CreateAsync(TEntity entity);
		Task UpdateAsync(TEntity entity, string id);
		Task DeleteAsync(string id);
  }
}
