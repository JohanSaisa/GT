using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Data.Data.GTAppDb
{
	public static class GTAppDataSeeder
	{
		private static Location Address1 { get; set; }
		private static Location Address2 { get; set; }
		private static Company Company1 { get; set; }
		private static Company Company2 { get; set; }
		private static Listing Listing1 { get; set; }
		private static Listing Listing2 { get; set; }
		private static ListingInquiry ListingInquiry1 { get; set; }
		private static ListingInquiry ListingInquiry2 { get; set; }
		private static ExperienceLevel ExperienceLevel1 { get; set; } = new ExperienceLevel() { Id = "exp1", Name = "Junior" };
		private static ExperienceLevel ExperienceLevel2 { get; set; } = new ExperienceLevel() { Id = "exp2", Name = "Intermediate" };


		public static void Initialize(IServiceProvider serviceProvider)
		{
			PopulateProperties();
			using (var context = new GTAppContext(
							serviceProvider.GetRequiredService<
											DbContextOptions<GTAppContext>>()))
			{
				SeedExperienceLevels(context);
				SeedAddresses(context);
				SeedCompanies(context);
				SeedListings(context);
				SeedListingInquiry(context);
			}
		}

		private static void SeedExperienceLevels(GTAppContext context)
		{
			if (context.ExperienceLevels.Any())
			{
				return;
			}
			context.ExperienceLevels.AddRange(ExperienceLevel1, ExperienceLevel2);
			context.SaveChanges();
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
			if (context.Locations.Any())
			{
				return;
			}
			context.Locations.AddRange(Address1, Address2);
			context.SaveChanges();
		}

		private static void PopulateProperties()
		{
			Address1 = new Location()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Garvargatan 3",
				Companies = new List<Company>()
			};
			Address2 = new Location()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Skärgårdsvägen 26B",
				Companies = new List<Company>()
			};
			Company1 = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Facebook",
				Locations = new List<Location>()
			};
			Company2 = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Google",
				Locations = new List<Location>()
			};
			Listing1 = new Listing()
			{
				Id = Guid.NewGuid().ToString(),
				ListingTitle = "Duktig .NET-Utvecklare till halvtidstjänst",
				Description = "Vi söker en duktig utvecklare till vår nya halvtidstjänst. Kommer jobba i ett litet team och utveckla applikationer efter kunders önskemål. Erfarenhet inom teknologioer som ASP.Net, Azure och Entity Framework är meriterande",
				Employer = Company1,
				SalaryMin = 30000,
				SalaryMax = 50000,
				JobTitle = ".Net-Utvecklare",
				Location = Address1,
				FTE = false,
				CreatedById = null,
				CreatedDate = DateTime.Now,
				ExperienceLevel = ExperienceLevel1
			};
			Listing2 = new Listing()
			{
				Id = Guid.NewGuid().ToString(),
				ListingTitle = "Spännande Robot AI-utvecklare sökes till Stockholm",
				Description = "Vill du vara del av vårt AI-team och jobba med värdelns främsta experter inom python och AI? Vi på Google söker just dig som vill vara en del av denna spännande branch. Du bör ha kunskaper inom AI-Development, Python och JS men alla är välkomna att söka",
				Employer = Company2,
				SalaryMin = 45000,
				SalaryMax = 45555,
				JobTitle = "AI-Developer",
				Location = Address2,
				FTE = true,
				CreatedById = null,
				CreatedDate = DateTime.Now,
				ExperienceLevel = ExperienceLevel2
			};
			ListingInquiry1 = new ListingInquiry()
			{
				Id = Guid.NewGuid().ToString(),
				MessageTitle = "Fullstack utvecklare med lång erfarenhet inom .Net",
				MessageBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
				LinkedInLink = "test@test.com",
				ApplicantId = null,
				Listing = Listing1
			};
			ListingInquiry2 = new ListingInquiry()
			{
				Id = Guid.NewGuid().ToString(),
				MessageTitle = "Nyexaminerad student söker er AI-Utvecklar tjänst",
				MessageBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
				LinkedInLink = "linkedin.com/användare2",
				ApplicantId = null,
				Listing = Listing2,
			};
			Company1.Locations.Add(Address1);
			Company2.Locations.Add(Address2);
			Address1.Companies.Add(Company1);
			Address2.Companies.Add(Company2);
		}
	}
}
