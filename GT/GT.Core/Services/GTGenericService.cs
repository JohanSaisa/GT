using GT.Core.DTO;
using GT.Data.Data;
using GT.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GT.Core.Services
{
  public abstract class GTGenericService<TEntity, TDataTransferObject>
  : IGTGenericService<TEntity, TDataTransferObject>
    where TEntity : class, IGTEntity
    where TDataTransferObject : class, IGTDataTransferObject
  {
    protected IGTGenericRepository<TEntity> _repository;

    protected GTGenericService(IGTGenericRepository<TEntity> repository)
    {
      _repository = repository;
    }

    public IQueryable<TDataTransferObject> Get(
      Expression<Func<TDataTransferObject, bool>>? predicateExpression = null, 
      Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null)
    {
      var items = _repository
        .GetAll(includeExpression)
        .Select(e => EntityToDTO(e));

      return predicateExpression is null
        ? items
        : items.Where(predicateExpression);
    }

    /// <summary>
    /// Gets the first entity in the database matching the given predicate, with optional includes.
    /// </summary>
    /// <param name="predicateExpression">Expression to find entity by.</param>
    /// <param name="includeExpression">Entity related data to be included.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<TDataTransferObject?> FirstOrDefaultAsync(
      Expression<Func<TDataTransferObject, bool>> predicateExpression, 
      Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null)
    {
      if (predicateExpression is null)
      {
        throw new ArgumentNullException(nameof(predicateExpression));
      }

      // Get entities with related data and return the first entity matching the given predicate.
      var item = await Get(predicateExpression, includeExpression)
        .FirstOrDefaultAsync(predicateExpression, CancellationToken.None);

      return item;
    }

    /// <summary>
    /// Finds a database entity by a set of keys. More efficient than finding by predicate, but cannot include related data.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual async Task<TDataTransferObject> FindAsync(params object[] keys)
    {
      var item = await _repository
        .FindAsync(keys);

      return EntityToDTO(item);
    }

    public virtual async Task<TDataTransferObject> CreateAsync(TDataTransferObject item)
    {
      var createdEntity = await _repository
        .AddAsync(DTOToEntity(item));

      return EntityToDTO(createdEntity);
    }

    public virtual async Task UpdateAsync(TDataTransferObject item, string id)
    {
      var entity = DTOToEntity(item);
      await _repository.UpdateAsync(entity, id);
    }

    public async Task DeleteAsync(string id)
    {
      await _repository.DeleteAsync(id);
    }

    protected abstract TDataTransferObject EntityToDTO(TEntity entity);

    protected abstract TEntity DTOToEntity(TDataTransferObject dataTransferObject);

  }
}
