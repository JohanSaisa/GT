using GT.Data.Data;
using GT.Data.Data.GTAppDb;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Repositories
{
	/// <summary>
	/// Base repository class.
	/// </summary>
	/// <typeparam name="TEntity">The database entity handled by the repository.</typeparam>
	public class GTGenericRepository<TEntity>
	: IGTGenericRepository<TEntity>
			where TEntity : class, IGTEntity
	{
		protected readonly GTAppContext _context;
		protected bool _disposed;

		public GTGenericRepository(GTAppContext context)
		{
			_context = context;
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return _context.Set<TEntity>();
		}

		/// <summary>
		/// Finds a database entity by a set of keys. More efficient than finding by predicate, but cannot include related data.
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public async Task<TEntity> FindAsync(params object[] keys)
		{
			return await _context
				.Set<TEntity>()
				.FindAsync(keys);
		}

		/// <summary>
		/// Adds a new entity to the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task<TEntity> AddAsync(TEntity entity)
		{
			if (entity is null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			try
			{
				await _context
					.Set<TEntity>()
					.AddAsync(entity);

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				throw;
			}

			return entity;
		}

		/// <summary>
		/// Updates an entity in the database.
		/// </summary>
		/// <param name="entity">Updated entity data.</param>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task UpdateAsync(TEntity entity, string id)
		{
			if (entity is null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			if (!ItemExists(id))
			{
				try
				{
					_context.Update(entity);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Deletes a database entity.
		/// </summary>
		/// <param name="id">ID of the entity to be deleted.</param>
		/// <returns></returns>
		public async Task DeleteAsync(string id)
		{
			var item = await _context
				.Set<TEntity>()
				.FirstOrDefaultAsync(e => e.Id == id);

			if (item is not null)
			{
				try
				{
					_context
						.Set<TEntity>()
						.Remove(item);

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					throw;
				}
			}
		}

		protected bool ItemExists(string id)
		{
			return _context
				.Set<TEntity>()
				.Any(e => e.Id == id);
		}

		//protected virtual void Dispose(bool disposing)
		//{
		//	if (!_disposed)
		//	{
		//		if (disposing)
		//		{
		//			_context.Dispose();
		//		}
		//	}

		//	_disposed = true;
		//}

		//public void Dispose()
		//{
		//	Dispose(true);
		//	GC.SuppressFinalize(this);
		//}
	}
}
