using GT.Data.Data.GTAppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestingNTier.DAL.Data
{
	public class GTAppContextFactory : IDesignTimeDbContextFactory<GTAppContext>
	{
		public GTAppContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<GTAppContext>();
			//TODO - Fix so that connection string imports from appsettings either in DATA or UI
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GTAppDb;Trusted_Connection=True;MultipleActiveResultSets=true");

			return new GTAppContext(optionsBuilder.Options);
		}
	}
}