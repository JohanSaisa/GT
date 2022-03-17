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
			var path = Directory.GetParent(typeof(Program).Assembly.Location).FullName;

			var builder = new ConfigurationBuilder()
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			IConfigurationRoot configuration = builder.Build();

			var optionsBuilder = new DbContextOptionsBuilder<GTIdentityContext>();

			optionsBuilder.UseSqlServer(configuration.GetConnectionString("GTIdentityContextConnection"));

			var context = new GTIdentityContext(optionsBuilder.Options);

			context.Database.EnsureCreated();
			return context;


			//var basePath = AppContext.BaseDirectory;

			//var cfg = new ConfigurationBuilder()
			//											.SetBasePath(basePath)
			//											.AddJsonFile("appsettings.json")
			//											.Build();

			//var optionsBuilder = new DbContextOptionsBuilder<GTIdentityContext>();

			//var connectionString = cfg.GetConnectionString("GTIdentityContextConnection");

			//optionsBuilder.UseSqlServer(connectionString);

			//return new GTIdentityContext(optionsBuilder.Options);

		}
	}
}