//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;

//namespace GT.Data.Repositories
//{
//  public abstract class GTGenericRepository<TEntity> : IGTGenericRepository<TEntity>
//    where TEntity : class, IGTEntity
//  {
//    protected DbContext _context;

//    protected GTGenericRepository<TEntity>(DbContext context) : base(context) { }

//  public async Task<IQueryable<TEntity>> GetAllAsync(
//    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
//  {
//    IQueryable<TEntity> query = _context.Set<TEntity>();

//    if (include is not null)
//    {
//      query = include(query);
//    }

//    var items = await query
//      .ToListAsync();

//    return items;
//  }

//  public Task<TEntity> CreateAsync(TEntity entity)
//  {
//    throw new NotImplementedException();
//  }

//  public Task UpdateAsync(TEntity entity, string id)
//  {
//    throw new NotImplementedException();
//  }

//  public Task DeleteAsync(string id)
//  {
//    throw new NotImplementedException();
//  }
//}
//}

