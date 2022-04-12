using GT.Data.Data.GTIdentityDb;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace GT.Data.Repositories.Impl
{
	public class GTIdentityRepository : IGTIdentityRepository
	{
		private readonly GTIdentityContext _context;
		private readonly ILogger<GTIdentityRepository> _logger;

		public GTIdentityRepository(GTIdentityContext context, ILogger<GTIdentityRepository> logger)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ApplicationUser> FindAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser)
		{
			var local = _context.Users.Local.FirstOrDefault(entity => entity.Id == applicationUser.Id);
			if (local is not null)
			{
				_context.Entry(local).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
			}
			_context.Entry(applicationUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			await _context.SaveChangesAsync();
			return applicationUser;
		}

		public async Task<ApplicationUser> AddAsync(ApplicationUser applicationUser)
		{
			applicationUser.Id = applicationUser.Id == "" ? Guid.NewGuid().ToString() : applicationUser.Id;
			_context.Add(applicationUser);
			_context.SaveChangesAsync();
			return applicationUser;
		}

		public async Task RemoveAsync(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user is not null)
			{
				_context.Users.Remove(user);
				await _context.SaveChangesAsync();
			}
		}

		public IQueryable<ApplicationUser> Get()
		{
			return _context.Users.AsQueryable();
		}
	}
}
