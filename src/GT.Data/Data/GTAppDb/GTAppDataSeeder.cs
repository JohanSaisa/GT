using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Data.Data.GTAppDb
{
	public static class GTAppDataSeeder
	{
		private static Address Address1 { get; set; }
		private static Address Address2 { get; set; }
		private static Company Company1 { get; set; }
		private static Company Company2 { get; set; }
		private static Listing Listing1 { get; set; }
		private static Listing Listing2 { get; set; }
		private static ListingInquiry ListingInquiry1 { get; set; }
		private static ListingInquiry ListingInquiry2 { get; set; }


		public static void Initialize(IServiceProvider serviceProvider)
		{
			PopulateProperties();
			using (var context = new GTAppContext(
							serviceProvider.GetRequiredService<
											DbContextOptions<GTAppContext>>()))
			{
				SeedAddresses(context);
				SeedCompanies(context);
				SeedListings(context);
				SeedListingInquiry(context);
			}
		}

		private static void SeedListingInquiry(GTAppContext context)
		{
			if (context.ListingInquiries.Any())
			{
				return;
			}
			context.ListingInquiries.AddRange(ListingInquiry1, ListingInquiry2);
			context.SaveChanges();
		}

		private static void SeedListings(GTAppContext context)
		{
			if (context.Listings.Any())
			{
				return;
			}
			context.Listings.AddRange(Listing1, Listing2);
			context.SaveChanges();
		}

		private static void SeedCompanies(GTAppContext context)
		{
			if (context.Companies.Any())
			{
				return;
			}
			context.Companies.AddRange(Company1, Company2);
			context.SaveChanges();
		}

		private static void SeedAddresses(GTAppContext context)
		{
			if (context.Addresses.Any())
			{
				return;
			}
			context.Addresses.AddRange(Address1, Address2);
			context.SaveChanges();
		}

		private static void PopulateProperties()
		{
			Address1 = new Address()
			{
				Id = Guid.NewGuid().ToString(),
				StreetAddress = "Garvargatan 3",
				ZipCode = "11226",
				Companies = new List<Company>()
			};
			Address2 = new Address()
			{
				Id = Guid.NewGuid().ToString(),
				StreetAddress = "Skärgårdsvägen 26B",
				ZipCode = "13931",
				Companies = new List<Company>()
			};
			Company1 = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Facebook",
				Addresses = new List<Address>()
			};
			Company2 = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Google",
				Addresses = new List<Address>()
			};
			Listing1 = new Listing()
			{
				Id = Guid.NewGuid().ToString(),
				ListingTitle = "Vi söker sexy .NET-Utvecklare",
				Description = "Fullstack utvecklare till vår nya spännande halvtidstjänst. Behöver vara otrologt smart och dutkig",
				Employer = Company1,
				SalaryMin = 30000,
				SalaryMax = 50000,
				JobTitle = ".Net-Utvecklare",
				Address = Address1,
				FTE = false,
				CreatedById = null,
				CreatedDate = DateTime.Now
			};
			Listing2 = new Listing()
			{
				Id = Guid.NewGuid().ToString(),
				ListingTitle = "Spännande Robot AI-utvecklare sökes till Stockholm",
				Description = "AI utvecklare med stenkoll på python och HTML",
				Employer = Company2,
				SalaryMin = 45000,
				SalaryMax = 45555,
				JobTitle = "AI-Developer",
				Address = Address2,
				FTE = true,
				CreatedById = null,
				CreatedDate = DateTime.Now
			};
			ListingInquiry1 = new ListingInquiry()
			{
				Id = Guid.NewGuid().ToString(),
				MessageTitle = "Jag vill ha jobb här tack",
				MessageBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
				LinkedInLink = "test@test.com",
				ApplicantId = null,
				ListingID = Listing1,
			};
			ListingInquiry2 = new ListingInquiry()
			{
				Id = Guid.NewGuid().ToString(),
				MessageTitle = "Ny examinerad student",
				MessageBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
				LinkedInLink = "linkedin.com/användare",
				ApplicantId = null,
				ListingID = Listing2,
			};

			Company1.Addresses.Add(Address1);
			Company2.Addresses.Add(Address2);
			Address1.Companies.Add(Company1);
			Address2.Companies.Add(Company2);
		}
	}
}