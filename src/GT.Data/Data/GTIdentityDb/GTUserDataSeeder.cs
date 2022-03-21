using GT.Data.Data.GTIdentityDb.Entities;
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
      SeedGTApplicationUser();
      SeedGTAdministrator();
#else
    Console.WriteLine("Mode=Release");
    SeedGtAdministrator();
#endif
    }

    /// <summary>
    /// Seed Identity DB with one Application User.
    /// </summary>
    private static void SeedGTApplicationUser()
    {
      (string userAccountGuid, string userRoleGuid) = GenerateAccountAndRoleGuids();

      CreateGTUserAccount(userAccountGuid);
      CreateGTUserRole(userRoleGuid);
      CoupleGTAccountAndRole(userAccountGuid, userRoleGuid);
    }

    /// <summary>
    /// Seed Identity DB with default Administrator.
    /// </summary>
    private static void SeedGTAdministrator()
    {
      (string adminAccountGuid, string adminRoleGuid) = GenerateAccountAndRoleGuids();

      CreateGTAdminAccount(adminAccountGuid);
      CreateGTAdminRole(adminRoleGuid);
      CoupleGTAdminAccountAndRole(adminAccountGuid, adminRoleGuid);
    }

    /// <summary>
    /// Generate Guids for Admin Account and Admin Role.
    /// </summary>
    /// <returns></returns>
    private static (string accountGuid, string roleGuid) GenerateAccountAndRoleGuids()
    {
      string accountGuid = Guid.NewGuid().ToString();
      string roleGuid = Guid.NewGuid().ToString();
      return (accountGuid, roleGuid);
    }

    /// <summary>
    /// Populate Identity DB with dummy user account.
    /// </summary>
    private static void CreateGTUserAccount(string userAccountGuid)
    {
      ApplicationUser gtUser = new ApplicationUser()
      {
        Id = userAccountGuid,
        UserName = "GTuser@user.com",
        Email = "GTuser@user.com",
        LockoutEnabled = false,
      };
      PasswordHasher<ApplicationUser> userPassword = new PasswordHasher<ApplicationUser>();
      gtUser.PasswordHash = userPassword.HashPassword(gtUser, "User123!");

      _builder.Entity<ApplicationUser>().HasData(gtUser);
    }

    /// <summary>
    /// Populate Identity DB with dummy user role.
    /// </summary>
    private static void CreateGTUserRole(string userRoleGuid)
    {
      var userRole = new IdentityRole()
      {
        Id = userRoleGuid,
        Name = "GTUser",
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
        NormalizedName = "GTUser"
      };

      _builder.Entity<IdentityRole>().HasData(userRole);
    }

    /// <summary>
    /// Connect the User account with User role in Identity DB.
    /// </summary>
    private static void CoupleGTAccountAndRole(string userAccountGuid, string userRoleGuid)
    {
      var appUser = new IdentityUserRole<string>()
      {
        UserId = userAccountGuid,
        RoleId = userRoleGuid
      };

      _builder.Entity<IdentityUserRole<string>>().HasData(appUser);
    }

    /// <summary>
    /// Populate Identity DB with Default Administrator.
    /// </summary>
    private static void CreateGTAdminAccount(string adminAccountGuid)
    {
      ApplicationUser gtAdmin = new ApplicationUser()
      {
        Id = adminAccountGuid,
        UserName = "GTadmin@admin.com",
        Email = "GTadmin@admin.com",
        LockoutEnabled = false,
      };
      PasswordHasher<ApplicationUser> adminPassword = new PasswordHasher<ApplicationUser>();
      gtAdmin.PasswordHash = adminPassword.HashPassword(gtAdmin, "Admin123!");

      _builder.Entity<ApplicationUser>().HasData(gtAdmin);
    }

    /// <summary>
    /// Populate Identity DB with default Administrator role.
    /// </summary>
    private static void CreateGTAdminRole(string adminRoleGuid)
    {
      var adminRole = new IdentityRole()
      {
        Id = adminRoleGuid,
        Name = "GTAdministrator",
        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
        NormalizedName = "GTAdministrator"
      };

      _builder.Entity<IdentityRole>().HasData(adminRole);
    }

    /// <summary>
    /// Connect the default Administrator account with Administrator role in Identity DB.
    /// </summary>
    private static void CoupleGTAdminAccountAndRole(string adminAccountGuid, string adminRoleGuid)
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