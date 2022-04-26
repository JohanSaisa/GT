﻿using GT.Data.Data;

namespace GT.Data.Repositories.Interfaces
{
	/// <summary>
	/// Defines what functionalities that need to be present in any implementation of a generic repository.
	/// </summary>
	/// <typeparam name="TEntity">The entity in the database represented by a given concrete repository implementation.</typeparam>
	public interface IGTGenericRepository<TEntity>
		where TEntity : class, IGTEntity
	{
		IQueryable<TEntity>? GetAll();
		Task<TEntity?> FindAsync(params object[] keys);
		Task<TEntity?> AddAsync(TEntity entity);
		Task UpdateAsync(TEntity entity, string id);
		Task DeleteAsync(TEntity entity);
	}
}
