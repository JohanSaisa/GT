using GT.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GT.Data.Repositories
{
  /// <summary>
  /// Base repository class.
  /// </summary>
  /// <typeparam name="TEntity">The database entity handled by the repository.</typeparam>
  public class GTGenericRepository<TEntity>
  : IGTGenericRepository<TEntity>, IDisposable
      where TEntity : class, IGTEntity
  {
    protected readonly DbContext _context;
    protected bool _disposed;

    protected GTGenericRepository(DbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Gets all entities of type TEntity from the database, with optional included related data.
    /// </summary>
    /// <param name="includeExpression">Entity related data to be included. Example:
    /// <br>
    /// var customers = GetAll(c => c.Include(c.Address).ThenInclude(a => a.City));
    /// </br></param>
    /// <returns>IQueryable containing all database entities.</returns>
    public virtual IQueryable<TEntity> GetAll(
        Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null)
    {
      // Check if any related data is to be included.
      if (includeExpression is not null)
      {
        // Compile the include expression and apply it to the relevant DbSet.
        var include = includeExpression.Compile();
        return include(_context.Set<TEntity>());
      }

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

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }

      _disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
