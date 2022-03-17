﻿using GT.Data.Data.GTAppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestingNTier.DAL.Data
{
	public class GTAppContextFactory : IDesignTimeDbContextFactory<GTAppContext>
	{
		public GTAppContext CreateDbContext(string[] args)
		{
			//TODO - Check to see if ~ will find the root folder. .sln?
			var basePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\", "GT.UI");

			var cfg = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json")
			.Build();

			var optionsBuilder = new DbContextOptionsBuilder<GTAppContext>();

			var connectionString = cfg.GetConnectionString("GTIdentityContextConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new GTAppContext(optionsBuilder.Options);
		}
	}
}