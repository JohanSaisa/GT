using GT.Data.Data;

namespace GT.Data.Repositories.Interfaces
{
	/// <summary>
	/// Defines what functionalities that need to be present in any implementation of a generic repository.
	/// </summary>
	/// <typeparam name="TEntity">The entity in the database represented by a given concrete repository implementation.</typeparam>
	public interface IGenericRepository<TEntity>
		where TEntity : class, IAppEntity
	{
		/// <summary>
		/// Gets entities of the generic type found in database. Supports includes.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		IQueryable<TEntity>? Get();

		/// <summary>
		/// Finds matching entity. Unable to include other entities. Faster than finding by predicate.
		/// </summary>
		/// <param name="keys"></param>
		/// <exception cref="ArgumentNullException"></exception>
		Task<TEntity?> FindAsync(params object[] keys);

		Task AddAsync(TEntity entity);

		void Update(TEntity entity);

		void Delete(TEntity entity);

		Task<bool> SaveAsync();
	}
}
