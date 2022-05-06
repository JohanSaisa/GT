using GT.Data.Data.AppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTAppDb
{
	public class AppContext : DbContext
	{
		public DbSet<Company> Companies { get; set; }
		public DbSet<ExperienceLevel> ExperienceLevels { get; set; }
		public DbSet<Listing> Listings { get; set; }
		public DbSet<Inquiry> ListingInquiries { get; set; }
		public DbSet<Location> Locations { get; set; }

		public AppContext(DbContextOptions<AppContext> options)
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
