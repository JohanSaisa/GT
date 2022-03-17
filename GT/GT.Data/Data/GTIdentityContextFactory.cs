using GT.Data.Data.GTIdentityDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestingNTier.DAL.Data
{
	public class GTIdentityContextFactory : IDesignTimeDbContextFactory<GTIdentityContext>
	{
		public GTIdentityContext CreateDbContext(string[] args)
		{
			//TODO - Check to see if ~ will find the root folder. .sln?
			var basePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\", "GT.UI");

			var cfg = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json")
			.Build();

			var optionsBuilder = new DbContextOptionsBuilder<GTIdentityContext>();

			var connectionString = cfg.GetConnectionString("GTIdentityContextConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new GTIdentityContext(optionsBuilder.Options);
		}
	}
}