using GT.Data.Data.GTIdentityDb.Entities;

namespace GT.Data.Repositories
{
	public interface IGTIdentityRepository
	{
		Task<ApplicationUser> AddAsync(ApplicationUser applicationUser);
		Task<ApplicationUser> FindAsync(string id);
		IQueryable<ApplicationUser> Get();
		Task RemoveAsync(string id);
		Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser);
	}
}
