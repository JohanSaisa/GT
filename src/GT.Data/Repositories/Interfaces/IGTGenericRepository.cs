using GT.Data.Data;

namespace GT.Data.Repositories.Interfaces
{
	/// <summary>
	/// Defines what functionalities that need to be present in any implementation of a generic repository.
	/// </summary>
	/// <typeparam name="TEntity">The entity in the database represented by a given concrete repository implementation.</typeparam>
	public interface IGTGenericRepository<TEntity>
		where TEntity : class, IGTEntity
	{
		/// <summary>
		/// Gets entities of the generic type found in database. Supports includes.
		/// </summary>
		/// <param name="entity"></param>
		/// <exception cref="ArgumentNullException"></exception>
		IQueryable<TEntity>? Get();

		/// <summary>
		/// Finds matching entity. Unable to include other entities.
		/// </summary>
		/// <param name="entity"></param>
		/// <exception cref="ArgumentNullException"></exception>
		Task<TEntity?> FindAsync(params object[] keys);

		/// <summary>
		/// Adds a new entity to the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <exception cref="ArgumentNullException"></exception>
		Task<TEntity?> AddAsync(TEntity entity);

		/// <summary>
		/// Updates an entity in the database.
		/// </summary>
		/// <param name="entity">Updated entity data.</param>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		Task UpdateAsync(TEntity entity, string id);

		/// <summary>
		/// Deletes a database entity.
		/// </summary>
		/// <param name="id">Entity to be deleted.</param>
		Task DeleteAsync(TEntity entity);
	}
}
