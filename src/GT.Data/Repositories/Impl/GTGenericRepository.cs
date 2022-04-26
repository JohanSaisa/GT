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
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public virtual IQueryable<TEntity>? GetAll()
		{
			try
			{
				return _context.Set<TEntity>();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
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
		public async Task<TEntity?> AddAsync(TEntity entity)
		{
			if (entity is not null)
			{
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
			else
			{
				_logger.LogWarning("Attempted to add a null reference entity to the database.");
				return null;
			}
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
			if (entity is not null && id is not null)
			{
				if (ItemExists(id))
				{
					try
					{
						_context.Entry(entity).State = EntityState.Modified;
						await _context.SaveChangesAsync();
					}
					catch (Exception e)
					{
						_logger.LogError(e.Message);
					}
				}
			}
			else
			{
				_logger.LogWarning("Attempted to update a null reference entity to the database.");
			}
		}

		/// <summary>
		/// Deletes a database entity.
		/// </summary>
		/// <param name="id">ID of the entity to be deleted.</param>
		/// <returns></returns>
		public async Task DeleteAsync(TEntity entity)
		{
			try
			{
				if (entity is not null)
				{
					_context
						.Set<TEntity>()
						.Remove(entity);

					await _context.SaveChangesAsync();
				}
				else
				{
					_logger.LogWarning("Attempted to update a null reference entity to the database.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

		protected bool ItemExists(string id)
		{
			if (id is null)
			{
				throw new ArgumentNullException(nameof(id));
			}

			try
			{
				return _context
					.Set<TEntity>()
					.Any(e => e.Id == id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}
		}
	}
}
