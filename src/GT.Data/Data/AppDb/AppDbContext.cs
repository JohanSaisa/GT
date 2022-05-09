using GT.Data.Data.AppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTAppDb
{
	public class AppDbContext : DbContext
	{
		public DbSet<Company> Companies => Set<Company>();
		public DbSet<ExperienceLevel> ExperienceLevels => Set<ExperienceLevel>();
		public DbSet<Listing> Listings => Set<Listing>();
		public DbSet<Inquiry> Inquiries => Set<Inquiry>();
		public DbSet<Location> Locations => Set<Location>();

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Company>()
				.HasIndex(e => e.Name)
				.IsUnique();

			builder.Entity<ExperienceLevel>()
				.HasIndex(e => e.Name)
				.IsUnique();

			builder.Entity<Location>()
				.HasIndex(e => e.Name)
				.IsUnique();

			builder.Entity<Location>()
				.HasMany(e => e.Companies)
				.WithMany(e => e.Locations)
				.UsingEntity(join => join.ToTable("CompanyLocations"));

			base.OnModelCreating(builder);
		}
	}
}
