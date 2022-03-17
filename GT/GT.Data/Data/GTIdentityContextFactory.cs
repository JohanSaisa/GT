using GT.Data.Data.GTIdentityDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestingNTier.DAL.Data
{
	public class GTIdentityContextFactory : IDesignTimeDbContextFactory<GTIdentityContext>
	{
		public GTIdentityContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<GTIdentityContext>();
			//TODO - Fix so that connection string imports from appsettings either in DATA or UI
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GTIdentityDb;Trusted_Connection=True;MultipleActiveResultSets=true");

			return new GTIdentityContext(optionsBuilder.Options);
		}
	}
}