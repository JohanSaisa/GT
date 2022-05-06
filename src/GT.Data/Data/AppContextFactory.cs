using GT.Data.Data.GTAppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestingNTier.DAL.Data
{
	public class AppContextFactory : IDesignTimeDbContextFactory<GTAppContext>
	{
		public GTAppContext CreateDbContext(string[] args)
		{
			var basePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\", "GT.UI");

			// TODO use connection string from user secrets
			var cfg = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json")
			.Build();

			var optionsBuilder = new DbContextOptionsBuilder<GTAppContext>();

			var connectionString = cfg.GetConnectionString("GTApplicationContextConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new GTAppContext(optionsBuilder.Options);
		}
	}
}
