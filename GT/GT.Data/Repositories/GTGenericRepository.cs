using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GT.Data.Repositories
{
    public abstract class GTGenericRepository<TEntity> : IGTGenericRepository<TEntity>, IDisposable
        where TEntity : class, IGTEntity
    {
        protected readonly DbContext _context;

        protected GTGenericRepository(DbContext context) 
        { 
            _context = context;
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include is not null)
            {
                query = include(query);
            }
            var items = await query
                .ToListAsync();

            return items
                .AsQueryable();
        }

        public Task<TEntity> FindAsync(Func<IQueryable<TEntity>, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Set<TEntity>().AddAsync(entity);

            return entity;
        }
        public Task UpdateAsync(TEntity entity, string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
