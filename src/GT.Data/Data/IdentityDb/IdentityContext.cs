using GT.Data.Data.IdentityDb.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.IdentityDb;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
	public IdentityContext(DbContextOptions<IdentityContext> options)
			: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		UserDataSeeder.InitializeUserSeeder(builder);
		// Customize the ASP.NET Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);
	}
}
