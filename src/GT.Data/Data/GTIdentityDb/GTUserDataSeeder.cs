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
		private static readonly string _gtAdminAccount = "GTadmin@admin.com";
		private static readonly string _gtUserAccount = "GTuser@user.com";
		private static readonly string _gtAdminRole = "GTadmin";
		private static readonly string _gtUserRole = "GTuser";
		private static readonly string _gtUserPassword = "User123!";
		private static readonly string _gtAdminPassword = "@dmin123!";
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
			SeedGTAdministrator();
			CreateGTUserRole(Guid.NewGuid().ToString());
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
				UserName = _gtUserAccount,
				NormalizedUserName = _gtUserAccount.ToUpper(),
				Email = _gtUserAccount,
				NormalizedEmail = _gtUserAccount.ToUpper(),
				EmailConfirmed = true,
				LockoutEnabled = false,
			};
			PasswordHasher<ApplicationUser> userPassword = new PasswordHasher<ApplicationUser>();
			gtUser.PasswordHash = userPassword.HashPassword(gtUser, _gtUserPassword);

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
				Name = _gtUserRole,
				ConcurrencyStamp = "2",
				NormalizedName = _gtUserRole.ToUpper(),
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
				UserName = _gtAdminAccount,
				NormalizedUserName = _gtAdminAccount.ToUpper(),
				Email = _gtAdminAccount,
				NormalizedEmail = _gtAdminAccount.ToUpper(),
				EmailConfirmed = true,
				LockoutEnabled = false,
			};
			PasswordHasher<ApplicationUser> adminPassword = new PasswordHasher<ApplicationUser>();
			gtAdmin.PasswordHash = adminPassword.HashPassword(gtAdmin, _gtAdminPassword);

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
				Name = _gtAdminRole,
				ConcurrencyStamp = "1",
				NormalizedName = _gtAdminRole.ToUpper(),
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
