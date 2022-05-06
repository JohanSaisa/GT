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
	public class GenericRepository<TEntity>
	: IGenericRepository<TEntity>
			where TEntity : class, IAppEntity
	{
		private readonly GTAppContext _context;

		public GenericRepository(GTAppContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IQueryable<TEntity>? Get()
		{
			return _context.Set<TEntity>();
		}
		
		public async Task<TEntity?> FindAsync(params object[] keys)
		{
			keys = keys
				.Where(key => key != null)
				.ToArray();
			
			return await _context
				.Set<TEntity>()
				.FindAsync(keys);

		}

		public async Task AddAsync(TEntity entity)
		{
			await _context
				.Set<TEntity>()
				.AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			_context.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			_context
				.Set<TEntity>()
				.Remove(entity);
		}

		public async Task<bool> SaveAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
