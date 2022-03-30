using GT.Data.Data;
using GT.Data.Data.GTAppDb;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Data.Repositories.Impl
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
		protected ILogger _logger;

		public GTGenericRepository(GTAppContext context, ILogger<GTGenericRepository<TEntity>> logger)
		{
			_context = context;
			_logger = logger;
		}

		public virtual IQueryable<TEntity>? GetAll()
		{
			try
			{
				return _context.Set<TEntity>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		/// <summary>
		/// Finds a database entity by a set of keys. More efficient than finding by predicate, but cannot include related data.
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public async Task<TEntity?> FindAsync(params object[] keys)
		{
			if (keys is null || !keys.Any())
			{
				return null;
			}

			keys = keys
				.Where(key => key != null)
				.ToArray();

			try
			{
				return await _context
					.Set<TEntity>()
					.FindAsync(keys);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
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
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
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

			if (id is null)
			{
				throw new ArgumentNullException(nameof(id));
			}

			if (!ItemExists(id))
			{
				try
				{
					_context.Update(entity);
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
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
			if(id is null)
			{
				throw new ArgumentNullException(nameof(id));
			}

			try
			{
				var item = await _context
					.Set<TEntity>()
					.FirstOrDefaultAsync(e => e.Id == id);

				if (item is not null)
				{
					_context
						.Set<TEntity>()
						.Remove(item);

					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

		protected bool ItemExists(string id)
		{
			if(id is null)
			{
				throw new ArgumentNullException(nameof(id));
			}

			try
			{
				return _context
					.Set<TEntity>()
					.Any(e => e.Id == id);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}		
		}
	}
}
