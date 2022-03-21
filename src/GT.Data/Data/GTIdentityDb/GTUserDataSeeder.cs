using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GT.Data.Data.GTIdentityDb
{
  public static class GTUserDataSeeder
  {
    private static ModelBuilder _builder;

    /// <summary>
    /// Populate Identity DB with default Admin and dummy User data.
    /// </summary>
    public static void InitializeUserSeeder(ModelBuilder builder)
    {
      _builder = builder;
#if DEBUG
      Console.WriteLine("Mode=Debug");
      SeedGtApplicationUser();
      SeedGtAdministrator();
#else
    Console.WriteLine("Mode=Release");
    SeedGtAdministrator();
#endif
    }

    /// <summary>
    /// Seed Identity DB with one Application User.
    /// </summary>
    private static void SeedGtApplicationUser()
    {
      SeedGtUser();
      SeedGtUserRole();
      CoupleGtUserAndRole();
    }

    /// <summary>
    /// Seed Identity DB with default Administrator.
    /// </summary>
    private static void SeedGtAdministrator()
    {
      (string adminAccountGuid, string adminRoleGuid) = GenerateAdminGuids();

      SeedGtAdmin(adminAccountGuid);
      SeedGtAdminRole(adminRoleGuid);
      CoupleGtAdminUserAndRole(adminAccountGuid, adminRoleGuid);
    }

    /// <summary>
    /// Generate Guids for Admin Account and Admin Role.
    /// </summary>
    /// <returns></returns>
    private static (string adminAccountGuid, string adminRoleGuid) GenerateAdminGuids()
    {
      string adminAccountGuid = Guid.NewGuid().ToString();
      string adminRoleGuid = Guid.NewGuid().ToString();
      return (adminAccountGuid, adminRoleGuid);
    }

    /// <summary>
    /// Populate Identity DB with dummy user account.
    /// </summary>
    private static void SeedGtUser()
    {
      IdentityUser gtUser = new IdentityUser()
      {
        Id = "d6498f73-89d2-458f-ba9a-013f4ca5e9dc",
        UserName = "GTuser@user.com",
        Email = "GTuser@user.com",
        LockoutEnabled = false,
      };
      PasswordHasher<IdentityUser> userPassword = new PasswordHasher<IdentityUser>();
      gtUser.PasswordHash = userPassword.HashPassword(gtUser, "User123!");

      _builder.Entity<IdentityUser>().HasData(gtUser);
    }

    /// <summary>
    /// Populate Identity DB with dummy user role.
    /// </summary>
    private static void SeedGtUserRole()
    {
      var userRole = new IdentityRole()
      {
        Id = "a263abf2-d66f-4193-b306-73b22894d51a",
        Name = "User",
        ConcurrencyStamp = "97b0bae1-87ac-430e-92cd-462c6f69f6cf",
        NormalizedName = "GTUser"
      };

      _builder.Entity<IdentityRole>().HasData(userRole);
    }

    /// <summary>
    /// Connect the User account with User role in Identity DB.
    /// </summary>
    private static void CoupleGtUserAndRole()
    {
      var appUser = new IdentityUserRole<string>()
      {
        RoleId = "a263abf2-d66f-4193-b306-73b22894d51a",
        UserId = "d6498f73-89d2-458f-ba9a-013f4ca5e9dc"
      };

      _builder.Entity<IdentityUserRole<string>>().HasData(appUser);
    }

    /// <summary>
    /// Populate Identity DB with Default Administrator.
    /// </summary>
    private static void SeedGtAdmin(string adminAccountGuid)
    {
      IdentityUser gtAdmin = new IdentityUser()
      {
        Id = adminAccountGuid,
        UserName = "GTadmin@admin.com",
        Email = "GTadmin@admin.com",
        LockoutEnabled = false,
      };
      PasswordHasher<IdentityUser> adminPassword = new PasswordHasher<IdentityUser>();
      gtAdmin.PasswordHash = adminPassword.HashPassword(gtAdmin, "Admin123!");

      _builder.Entity<IdentityUser>().HasData(gtAdmin);
    }

    /// <summary>
    /// Populate Identity DB with default Administrator role.
    /// </summary>
    private static void SeedGtAdminRole(string adminRoleGuid)
    {
      var adminRole = new IdentityRole()
      {
        Id = adminRoleGuid,
        Name = "Admin",
        ConcurrencyStamp = "9cc023c3-d41d-4d57-85fe-9fa73578c522",
        NormalizedName = "GTAdministrator"
      };

      _builder.Entity<IdentityRole>().HasData(adminRole);
    }

    /// <summary>
    /// Connect the default Administrator account with Administrator role in Identity DB.
    /// </summary>
    private static void CoupleGtAdminUserAndRole(string adminAccountGuid, string adminRoleGuid)
    {
      var adminUser = new IdentityUserRole<string>()
      {
        UserId = adminAccountGuid,
        RoleId = adminRoleGuid
      };

      _builder.Entity<IdentityUserRole<string>>().HasData(adminUser);
    }
  }
}