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
			base.OnModelCreating(builder);
		}
	}
}
