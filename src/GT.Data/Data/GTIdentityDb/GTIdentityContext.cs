using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GT.Data.Data.GTIdentityDb;

public class GTIdentityContext : IdentityDbContext<ApplicationUser>
{
  public GTIdentityContext(DbContextOptions<GTIdentityContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    GTUserDataSeeder.InitializeUserSeeder(builder);
    // Customize the ASP.NET Identity model and override the defaults if needed.
    // For example, you can rename the ASP.NET Identity table names and more.
    // Add your customizations after calling base.OnModelCreating(builder);
  }
}