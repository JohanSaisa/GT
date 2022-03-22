using GT.Core.DTO;
using GT.Data.Data;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GT.Core.Services
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity">The database entity handled by the service.</typeparam>
	/// <typeparam name="TDataTransferObject">The DTO representing the database entity handled by the service.</typeparam>
	public interface IGTGenericService<TEntity, TDataTransferObject>
		where TEntity : class, IGTEntity
		where TDataTransferObject : class, IGTDataTransferObject
	{
		IQueryable<TDataTransferObject> GetAll(
			Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null);

		IQueryable<TDataTransferObject> Filter(
			Expression<Func<TDataTransferObject, bool>> predicateExpression,
			Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null);

		Task<TDataTransferObject?> FirstOrDefaultAsync(
			Expression<Func<TDataTransferObject, bool>> predicate,
			Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null);

		Task<TDataTransferObject> FindAsync(params object[] keys);
		Task<TDataTransferObject> CreateAsync(TDataTransferObject item);
		Task UpdateAsync(TDataTransferObject item, string id);
		Task DeleteAsync(string id);
	}
}
