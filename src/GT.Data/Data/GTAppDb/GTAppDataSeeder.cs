using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Data.Data.GTAppDb
{
	public static class GTAppDataSeeder
	{
		// Number of Data objects.
		private static readonly int _numberOfItems = 11;

		// Magic numbers for Company names, 11 items.
		private static readonly List<string> _companyNames = new List<string>()
			{
				"Fake Company AB",
				"Animal News Inc.",
				"IT 4 Solutions",
				"Employee Central Inc.",
				"Unfaked",
				"Klarenea AB",
				"Softwared",
				"Blue Pill",
				"The Binary Library",
				"Foody Thoughty",
				"Millenials Flash Resort",
			};

		// Magic numbers for Locations, 11 items.
		private static readonly List<string> _locationNames = new List<string>()
			{
				"Stockholm",
				"Remote",
				"Malmö",
				"Göteborg",
				"Umeå",
				"Karlstad",
				"Åbo",
				"Gnesta",
				"Karlskrona",
				"Karlskoga",
				"Örkelljunga",
			};

		// Magic numbers for Listing titles, 11 items.
		private static readonly List<string> _listingTitles = new List<string>()
			{
				"Duktig .NET-Utvecklare till halvtidstjänst",
				"Spännande Robot AI-utvecklare sökes till Stockholm",
				"Vill du hjälpa oss att planlägga vår framtida kundbas?",
				"Är du en projektledare som drivs av snabba projekt?",
				"Vi söker CIO för Nordiskt försäkringsbolag",
				"We are looking for a senior project manager to head our Project office",
				"Certifierad SCRUM Master med erfarenhet inom Finans sökes!",
				"Kan du Java, speldesign och trivs att arbeta i en familjär miljö?",
				"Vi söker dig som kan React",
				"Our Salesforce team is expanding at Food for thought that you bought.",
				"Vi söker dig som kan orkestrera stora upphandlingar gentemot offentlig verksamhet",
			};

		// Magic numbers for Listing descriptions, 11 items.
		private static readonly List<string> _listingDescriptions = new List<string>()
			{
				"Vi söker en duktig utvecklare till vår nya halvtidstjänst. Kommer jobba i ett litet team och utveckla applikationer efter kunders önskemål. Erfarenhet inom teknologioer som ASP.Net, Azure och Entity Framework är meriterande",
				"Vill du vara del av vårt AI-team och jobba med värdelns främsta experter inom python och AI? Vi på Google söker just dig som vill vara en del av denna spännande branch. Du bör ha kunskaper inom AI-Development, Python och JS men alla är välkomna att söka",
				"ML: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel. Mauris sed leo augue. Suspendisse pretium imperdiet enim. Aliquam in molestie odio, ut ornare lacus. In gravida.",
				"PM for small infrastructure projects: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"CIO for Nordic Insurance company: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"Head of Project Office: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"SCRUM Master in Finance: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"Java Developer small gaming company: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"Front end developer React: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"Salesforce expert: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
				"Bid manager in Public Procurement: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in elementum diam. Suspendisse consectetur sed nunc quis suscipit. Fusce dignissim lectus a luctus laoreet. Donec varius magna ex, in pretium urna pharetra vel.",
			};

		// Magic numbers for Job titles for the Listing, 11 items.
		private static readonly List<string> _jobTitles = new List<string>()
			{
				".Net-Utvecklare",
				"AI developer",
				"ML in Python",
				"PM for small infrastructure projects",
				"CIO for Nordic Insurance company",
				"Head of Project Office",
				"SCRUM Master in Finance",
				"Java Developer small gaming company",
				"Front end developer React",
				"Salesforce expert",
				"Bid manager in Public Procurement",
			};

		// Magic numbers for Listing Inquiry messages titles, 11 items.
		private static readonly List<string> _messageTitles = new List<string>()
			{
				"Fullstack utvecklare med 5års erfarenhet inom .Net",
				"Nyexaminerad student söker er AI-Utvecklar tjänst",
				"Machine learning guru looking for opportunities in Environmental agencies",
				"Project Manager with 4 years of experience in Aviation industry",
				"Senior exec with experience from consulting industry",
				"Programme manager and CTO with 20+ years of experience",
				"Certified SCRUM Master with previous experience in Finance",
				"Java Developer looking to develop the next minecraft",
				"Front end developer with expertise in React, Angular and JavaScript",
				"Certified Advanced Administrator in Salesforce looking to connect",
				"Senior manager with experience in large scale public procurement bids",
			};

		// Magic numbers for Experience levels, 11 items.
		private static readonly List<string> _experienceTitles = new List<string>()
			{
				"Entry level",
				"Internship",
				"Mid-Senior level",
				"Director",
				"Entryer level",
				"Mid-late-Senior level",
				"Mid-last-Senior level",
				"Warlock",
				"Manager",
				"Assistant manager",
				"Assistant to the manager",
			};

		/// <summary>
		/// Initialize data seeder, adding dummy data and mapping objects.
		/// </summary>
		/// <param name="serviceProvider"></param>
		public static async void Initialize(IServiceProvider serviceProvider)
		{
			List<Company> tempCompanies = PopulateCompanies();
			List<ExperienceLevel> experienceLevels = PopulateExperienceLevels();
			List<Location> tempLocations = PopulateLocations();

			// Junction table connections.
			(List<Company> companies, List<Location> locations) = SetCompanyLocations(tempCompanies, tempLocations);
			List<Listing> listings = PopulateListings(companies, experienceLevels, locations);
			List<ListingInquiry> listingInquiries = PopulateListingInquiries(listings);

			using (var context = new GTAppContext(
							serviceProvider.GetRequiredService<DbContextOptions<GTAppContext>>()))
			{
				SeedDataToDatabase(context, companies, listingInquiries, listings, locations);
			}
		}

		/// <summary>
		/// Create list of companies.
		/// </summary>
		/// <returns></returns>
		private static List<Company> PopulateCompanies()
		{
			List<Company> companies = new List<Company>();

			for (int i = 0; i < _numberOfItems; i++)
			{
				var company = new Company()
				{
					Id = Guid.NewGuid().ToString(),
					Name = _companyNames[i],
					Locations = new List<Location>()
				};

				companies.Add(company);
			}

			return companies;
		}

		/// <summary>
		/// Create list of Experience levels
		/// </summary>
		/// <returns></returns>
		private static List<ExperienceLevel> PopulateExperienceLevels()
		{
			List<ExperienceLevel> experienceLevels = new List<ExperienceLevel>();

			for (int i = 0; i < _numberOfItems; i++)
			{
				var experienceLevel = new ExperienceLevel()
				{
					Id = Guid.NewGuid().ToString(),
					Name = _experienceTitles[i]
				};

				experienceLevels.Add(experienceLevel);
			}

			return experienceLevels;
		}

		/// <summary>
		/// Create list of locations.
		/// </summary>
		/// <returns></returns>
		private static List<Location> PopulateLocations()
		{
			List<Location> locations = new List<Location>();

			for (int i = 0; i < _numberOfItems; i++)
			{
				var location = new Location()
				{
					Id = Guid.NewGuid().ToString(),
					Name = _locationNames[i],
					Companies = new List<Company>()
				};

				locations.Add(location);
			}

			return locations;
		}

		/// <summary>
		/// Mapping the Companies to Locations and vice versa, returning the related objects as lists.
		/// </summary>
		/// <param name="companies"></param>
		/// <param name="locations"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		private static (List<Company>, List<Location>) SetCompanyLocations(List<Company> companies, List<Location> locations)
		{
			if (companies.Count <= 0 || companies == null || locations.Count <= 0 || locations == null)
			{
				throw new ArgumentNullException();
			}

			List<Company> updatedCompanies = new List<Company>();
			List<Location> updatedLocations = new List<Location>();

			for (int i = 0; i < _numberOfItems; i++)
			{
				companies[i].Locations.Add(locations[i]);
				locations[i].Companies.Add(companies[i]);
				updatedCompanies.Add(companies[i]);
				updatedLocations.Add(locations[i]);
			}

			return (updatedCompanies, updatedLocations);
		}

		/// <summary>
		/// Map the listings with companies and locations.
		/// </summary>
		/// <param name="companies"></param>
		/// <param name="locations"></param>
		/// <returns></returns>
		private static List<Listing> PopulateListings(List<Company> companies, List<ExperienceLevel> experienceLevels, List<Location> locations)
		{
			List<Listing> listings = new List<Listing>();

			var rand = new Random();
			int salaryMin;
			int salaryMax;
			bool fte;

			for (int i = 0; i < _listingTitles.Count; i++)
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
					ListingTitle = _listingTitles[i],
					Description = _listingDescriptions[i],
					Employer = companies[i],
					SalaryMin = salaryMin,
					SalaryMax = salaryMax,
					JobTitle = _jobTitles[i],
					Location = locations[i],
					FTE = fte,
					CreatedById = null,
					CreatedDate = DateTime.Now,
					ExperienceLevel = experienceLevels[i],
					ApplicationDeadline = DateTime.Now.AddDays(rand.Next(40, 201))
				};

				listings.Add(listing);
			}

			return listings;
		}

		/// <summary>
		/// Create list of ListingInquiries and mapping a Listing to it.
		/// </summary>
		/// <param name="listings"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		private static List<ListingInquiry> PopulateListingInquiries(List<Listing> listings)
		{
			if (listings.Count <= 0 || listings == null)
			{
				throw new ArgumentNullException();
			}
			List<ListingInquiry> listingInquiries = new List<ListingInquiry>();

			List<string> messageBodies = new List<string>();
			for (int i = 0; i < _numberOfItems; i++)
			{
				string message = $"I am person #{i + 1}. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud";
				messageBodies.Add(message);
			}

			List<string> linkedInLinks = new List<string>();
			for (int i = 0; i < _numberOfItems; i++)
			{
				string linkedInLink = $"linkedin.com/IAmNotReallyAPerson{i + 1}";
				linkedInLinks.Add(linkedInLink);
			}

			for (int i = 0; i < _numberOfItems; i++)
			{
				var listingInquiry = new ListingInquiry()
				{
					Id = Guid.NewGuid().ToString(),
					MessageTitle = _messageTitles[i],
					MessageBody = messageBodies[i],
					LinkedInLink = linkedInLinks[i],
					ApplicantId = null,
					Listing = listings[i],
				};

				listingInquiries.Add(listingInquiry);
			}

			return listingInquiries;
		}

		/// <summary>
		/// Save the generated objects to the database.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="companies"></param>
		private static void SeedDataToDatabase(GTAppContext context, List<Company> companies, List<ListingInquiry> listingInquiries, List<Listing> listings, List<Location> locations)
		{
			if (context.Listings.Any())
			{
				return;
			}

			context.Companies.AddRange(companies);
			context.ListingInquiries.AddRange(listingInquiries);
			context.Listings.AddRange(listings);
			context.Locations.AddRange(locations);
			context.SaveChanges();
		}
	}
}
