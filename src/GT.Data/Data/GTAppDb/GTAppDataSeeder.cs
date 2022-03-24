using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Data.Data.GTAppDb
{
	public static class GTAppDataSeeder
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			List<Company> tempCompanies = PopulateCompanies();
			List<Location> tempLocations = PopulateLocations();
			List<Listing> listings = PopulateListings(tempCompanies, tempLocations);
			List<ListingInquiry> listingInquiries = PopulateListingInquiries();
			(List<Company> companies, List<Location> locations) = SetCompanyLocations(tempCompanies, tempLocations);

			using (var context = new GTAppContext(
							serviceProvider.GetRequiredService<
											DbContextOptions<GTAppContext>>()))
			{
				SeedCompanies(context, companies);
				SeedListings(context, listings);
				SeedListingInquiry(context, listingInquiries);
				SeedLocations(context, locations);
			}
		}

		private static List<Company> PopulateCompanies()
		{
			List<Company> companies = new List<Company>();
			List<string> companyNames = new List<string>()
			{
				"Fake Company delivering Chemicals",
				"My News Site For Animals",
				"Another Company in IT",
				"We are looking for Employees",
				"This is not a fake company",
				"We love IT and so should you",
				"Hard software for your 4th gen device",
				"Take the blue pill",
				"The Binary Library",
				"Food for thought that you bought",
				"Flashy key rings for the Millenia",
			};

			for (int i = 0; i < companyNames.Count; i++)
			{
				var company = new Company()
				{
					Id = Guid.NewGuid().ToString(),
					Name = companyNames[i],
					Locations = new List<Location>()
				};

				companies.Add(company);
			}

			return companies;
		}

		private static List<Location> PopulateLocations()
		{
			List<Location> locations = new List<Location>();
			List<string> locationNames = new List<string>()
			{
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
			};

			for (int i = 0; i < locationNames.Count; i++)
			{
				var location = new Location()
				{
					Id = Guid.NewGuid().ToString(),
					Name = locationNames[i],
					Companies = new List<Company>()
				};

				locations.Add(location);
			}

			return locations;
		}

		private static List<Listing> PopulateListings(List<Company> companies, List<Location> locations)
		{
			List<Listing> listings = new List<Listing>();
			List<string> listingTitles = new List<string>()
			{
				"Duktig .NET-Utvecklare till halvtidstjänst",
				"Spännande Robot AI-utvecklare sökes till Stockholm",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
			};
			List<string> listingDescriptions = new List<string>()
			{
				"Vi söker en duktig utvecklare till vår nya halvtidstjänst. Kommer jobba i ett litet team och utveckla applikationer efter kunders önskemål. Erfarenhet inom teknologioer som ASP.Net, Azure och Entity Framework är meriterande",
				"Vill du vara del av vårt AI-team och jobba med värdelns främsta experter inom python och AI? Vi på Google söker just dig som vill vara en del av denna spännande branch. Du bör ha kunskaper inom AI-Development, Python och JS men alla är välkomna att söka",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
			};
			List<string> jobTitles = new List<string>()
			{
				".Net-Utvecklare",
				"AI developer",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
			};

			var rand = new Random();
			int salaryMin;
			int salaryMax;
			bool fte;

			for (int i = 0; i < listingTitles.Count; i++)
			{
				salaryMin = rand.Next(10000, 40000);
				salaryMax = rand.Next(salaryMin, 70000);
				fte = false;
				if (rand.Next(2) == 0)
				{
					fte = true;
				}

				var listing = new Listing()
				{
					Id = Guid.NewGuid().ToString(),
					ListingTitle = listingTitles[i],
					Description = listingDescriptions[i],
					Employer = companies[i],
					SalaryMin = salaryMin,
					SalaryMax = salaryMax,
					JobTitle = jobTitles[i],
					Location = locations[i],
					FTE = fte,
					CreatedById = null,
					CreatedDate = DateTime.Now
				};

				listings.Add(listing);
			}

			return listings;
		}

		private static List<ListingInquiry> PopulateListingInquiries(List<Listing> listings)
		{
			if (listings.Count <= 0 || listings == null)
			{
				throw new ArgumentNullException();
			}
			List<ListingInquiry> listingInquiries = new List<ListingInquiry>();
			List<string> messageTitles = new List<string>()
			{
				"Fullstack utvecklare med lång erfarenhet inom .Net",
				"Nyexaminerad student söker er AI-Utvecklar tjänst",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
			};
			List<string> messageBodies = new List<string>();
			for (int i = 0; i < messageTitles.Count; i++)
			{
				string message = $"I am person #{i + 1}. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud";
				messageBodies.Add(message);
			}

			List<string> linkedInLinks = new List<string>();
			for (int i = 0; i < messageTitles.Count; i++)
			{
				string linkedInLink = $"linkedin.com/IAmNotReallyAPerson{i + 1}";
				linkedInLinks.Add(linkedInLink);
			}

			for (int i = 0; i < messageTitles.Count; i++)
			{
				var listingInquiry = new ListingInquiry()
				{
					Id = Guid.NewGuid().ToString(),
					MessageTitle = messageTitles[i],
					MessageBody = messageBodies[i],
					LinkedInLink = linkedInLinks[i],
					ApplicantId = null,
					Listing = listings[i],
				};

				listingInquiries.Add(listingInquiry);
			}

			return listingInquiries;
		}

		private static (List<Company>, List<Location>) SetCompanyLocations(List<Company> companies, List<Location> locations)
		{
			if (companies.Count <= 0 || companies == null || locations.Count <= 0 || locations == null)
			{
				throw new ArgumentNullException();
			}

			int listLength = companies.Count;
			if (listLength < locations.Count)
			{
				listLength = locations.Count;
			}

			List<Company> updatedCompanies = new List<Company>();
			List<Location> updatedLocations = new List<Location>();

			for (int i = 0; i < listLength - 1; i++)
			{
				companies[i].Locations.Add(locations[i]);
				locations[i].Companies.Add(companies[i]);
				updatedCompanies.Add(companies[i]);
				updatedLocations.Add(locations[i]);
			}

			return (updatedCompanies, updatedLocations);
		}

		private static async void SeedCompanies(GTAppContext context, List<Company> companies)
		{
			if (context.Companies.Any())
			{
				return;
			}
			context.Companies.AddRange(companies);
			await context.SaveChangesAsync();
		}

		private static async void SeedListingInquiry(GTAppContext context, List<ListingInquiry> listingInquiries)
		{
			if (context.ListingInquiries.Any())
			{
				return;
			}
			context.ListingInquiries.AddRange(listingInquiries);
			await context.SaveChangesAsync();
		}

		private static async void SeedListings(GTAppContext context, List<Listing> listings)
		{
			if (context.Listings.Any())
			{
				return;
			}
			context.Listings.AddRange(listings);
			await context.SaveChangesAsync();
		}

		private static async void SeedLocations(GTAppContext context, List<Location> locations)
		{
			if (context.Locations.Any())
			{
				return;
			}
			context.Locations.AddRange(locations);
			await context.SaveChangesAsync();
		}
	}
}
