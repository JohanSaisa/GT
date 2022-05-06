using GT.Data.Data.GTAppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GT.Data.Data
{
	public class AppContextFactory : IDesignTimeDbContextFactory<GTAppDb.AppContext>
	{
		public GTAppDb.AppContext CreateDbContext(string[] args)
		{
			var basePath = Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\", "GT.UI");

			// TODO use connection string from user secrets
			var cfg = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json")
			.Build();

			var optionsBuilder = new DbContextOptionsBuilder<GTAppDb.AppContext>();

			var connectionString = cfg.GetConnectionString("GTApplicationContextConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new GTAppDb.AppContext(optionsBuilder.Options);
		}
	}
}
