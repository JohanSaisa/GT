using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTAppDb
{
	public class GTAppContext : DbContext
	{
		public DbSet<Company> Companies { get; set; }
		public DbSet<ExperienceLevel> ExperienceLevels { get; set; }
		public DbSet<Listing> Listings { get; set; }
		public DbSet<ListingInquiry> ListingInquiries { get; set; }
		public DbSet<Location> Locations { get; set; }

		public GTAppContext(DbContextOptions<GTAppContext> options)
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

			base.OnModelCreating(builder);
		}
	}
}
