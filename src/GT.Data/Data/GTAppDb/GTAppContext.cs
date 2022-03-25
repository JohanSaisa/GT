using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTAppDb
{
	public class GTAppContext : DbContext
	{
		internal DbSet<Company> Companies { get; set; }
		internal DbSet<ExperienceLevel> ExperienceLevels { get; set; }
		public DbSet<Listing> Listings { get; set; }
		internal DbSet<ListingInquiry> ListingInquiries { get; set; }
		internal DbSet<Location> Locations { get; set; }

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
