using GT.Data.Data.GTAppDb.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GT.Data.Data.GTAppDb
{
	public static class GTAppDataSeeder
	{
		private static ModelBuilder _builder;
		private static Address Address1 { get; set; } 
		private static Address Address2 { get; set; }
		private static Company Company1 { get; set; }
		private static Company Company2 { get; set; }
		private static Listing Listing1 { get; set; }
		private static Listing Listing2 { get; set; }
		private static ListingInquiry ListingInquiry1 { get; set; }
		private static ListingInquiry ListingInquiry2 { get; set; }
		private static AddressCompany AdressCompany1 { get; set; }
		private static AddressCompany AdressCompany2 { get; set; }



		public static void InitializeAppDataSeeder(ModelBuilder builder)
		{
			_builder = builder;
			PopulateProperties();
			SeedAddressCompany();
			SeedAddresses();
			SeedCompanies();
			SeedListings();
			SeedListingInquiries();
			
		}

		

		/// <summary>
		/// Populate properties to seed the database
		/// </summary>
		private static void PopulateProperties()
		{
			(string addressGuid, string addressGuid2, string companyGuid, string companyGuid2, string listingGuid, string listingGuid2) = GenerateGuids();

			Address1 = new Address()
			{
				Id = addressGuid,
				StreetAddress = "Garvargatan 3",
				ZipCode = "11226",
				Companies = new List<Company>()
			};
			Address2 = new Address()
			{
				Id = addressGuid2,
				StreetAddress = "Skärgårdsvägen 26B",
				ZipCode = "13931",
				Companies = new List<Company>()
			};
			Company1 = new Company()
			{
				Id = companyGuid,
				Name = "Facebook",
				Addresses = new List<Address>()
			};
			Company2 = new Company()
			{
				Id = companyGuid2,
				Name = "Google",
				Addresses = new List<Address>()
			};
			Listing1 = new Listing()
			{
				Id = listingGuid,
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
				Id = listingGuid2,
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
			AdressCompany1 = new AddressCompany()
			{
				Id = Guid.NewGuid().ToString(),
				CompanyId = Company1.Id,
				AddressId = Address1.Id
			};
			AdressCompany2 = new AddressCompany()
			{
				Id = Guid.NewGuid().ToString(),
				CompanyId = Company2.Id,
				AddressId = Address2.Id
			};
		}
		/// <summary>
		/// Generate guids
		/// </summary>
		private static (string addressGuid, string addressGuid2, string companyGuid, string companyGuid2, string listingGuid, string listingGuid2) GenerateGuids()
		{
			string addressGuid = Guid.NewGuid().ToString();
			string addressGuid2 = Guid.NewGuid().ToString();
			string companyGuid = Guid.NewGuid().ToString();
			string companyGuid2 = Guid.NewGuid().ToString();
			string listingGuid = Guid.NewGuid().ToString();
			string listingGuid2 = Guid.NewGuid().ToString();
			return (addressGuid, addressGuid2, companyGuid, companyGuid2, listingGuid, listingGuid2);
		}

		private static void SeedAddresses()
		{
			_builder.Entity<Address>().HasData(Address1);
			_builder.Entity<Address>().HasData(Address2);
		}

		private static void SeedCompanies()
		{
			_builder.Entity<Company>().HasData(Company1);
			_builder.Entity<Company>().HasData(Company2);
		}

		private static void SeedListings()
		{
			_builder.Entity<Listing>().HasData(Listing1);
			_builder.Entity<Listing>().HasData(Listing2);
		}

		private static void SeedListingInquiries()
		{
			_builder.Entity<ListingInquiry>().HasData(ListingInquiry1);
			_builder.Entity<ListingInquiry>().HasData(ListingInquiry2);
		}
		private static void SeedAddressCompany()
		{
			Address1.Companies.Add(Company1);
			Address2.Companies.Add(Company2);
			Company1.Addresses.Add(Address1);
			Company2.Addresses.Add(Address2);


			_builder.Entity<AddressCompany>().HasData(AdressCompany1);
			_builder.Entity<AddressCompany>().HasData(AdressCompany2);
		}
	}
}

//using (var scope = app.Services.CreateScope())
//{
//  var services = scope.ServiceProvider;
//  SeedData.Initialize(services);
//}