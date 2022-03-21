using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTAppDb
{
  public class GTAppContext : DbContext
  {
    internal DbSet<Address> Addresses { get; set; }
    internal DbSet<Company> Companies { get; set; }
    internal DbSet<Listing> Listings { get; set; }
    internal DbSet<ListingInquiry> ListingInquiries { get; set; }

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