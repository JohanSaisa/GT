using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GT.Data.Repositories
{
    public abstract class GTGenericRepository<TEntity> : IGTGenericRepository<TEntity>, IDisposable
        where TEntity : class, IGTEntity
    {
        protected readonly DbContext _context;
        protected bool _disposed;

        protected GTGenericRepository(DbContext context) 
        { 
            _context = context;
        }

        public virtual IQueryable<TEntity> GetAll(
            Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null)
        {
            if (includeExpression is not null)
            {
                var include = includeExpression.Compile();

                return include(_context.Set<TEntity>());
            }
            
            return _context.Set<TEntity>();
        }

        // JobListingRepository.FindAsync(jobListing =>
        // jobListing.Id == id,
        // jb => jb.Include(x => x.Company));

        public virtual async Task<TEntity?> FindAsync(
            Expression<Func<TEntity, bool>> predicate, 
            Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includeExpression = null)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var item = await GetAll(includeExpression)
                .FirstOrDefaultAsync(predicate, CancellationToken.None);

            return item;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if(entity is null)
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
            catch(DbUpdateException)
            {
                throw;
            }

            return entity;
        }
        public async Task UpdateAsync(TEntity entity, string id)
        {
            if(entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if(!ItemExists(id))
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        public async Task Delete(string id)
        {
            var item = await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id);

            if(item is not null)
            {
                try
                {
                    _context
                        .Set<TEntity>()
                        .Remove(item);

                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException)
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
            if(!_disposed)
            {
                if(disposing)
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
