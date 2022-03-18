using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GT.Data.Repositories
{
	public interface IGTGenericRepository<TEntity>
		where TEntity : class, IGTEntity
	{
		IQueryable<TEntity> GetAll(
			Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? include);

		Task<TEntity?> FindAsync(
			Expression<Func<TEntity, bool>> predicate,
			Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? include);

		Task<TEntity> CreateAsync(TEntity entity);
		Task UpdateAsync(TEntity entity, string id);
		Task Delete(string id);
  }
}
