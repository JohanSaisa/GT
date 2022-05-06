using GT.Data.Data.IdentityDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestingNTier.DAL.Data
{
	public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
	{
		public IdentityContext CreateDbContext(string[] args)
		{
			var basePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\", "GT.UI");

			// TODO use connection string from user secrets
			var cfg = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json")
			.Build();

			var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

			var connectionString = cfg.GetConnectionString("GTIdentityContextConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new IdentityContext(optionsBuilder.Options);
		}
	}
}
