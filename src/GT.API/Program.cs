using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTIdentityDb;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories.Impl;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;
var identityConnectionString = configuration["GTIdentityContextConnection"];
var appConnectionString = configuration["GTApplicationContextConnection"];

builder.Services
	.AddDbContext<GTAppContext>(options =>
		options.UseSqlServer(appConnectionString))
	.AddDbContext<GTIdentityContext>(options =>
		options.UseSqlServer(identityConnectionString));

// Add identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	options.Password.RequireDigit = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 8;
})
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<GTIdentityContext>();

// Add services to the container.
builder.Services
	.AddControllers()
	.AddNewtonsoftJson(o =>
	{
		o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	});

// Add DAL repositories
builder.Services
	.AddScoped(typeof(IGTGenericRepository<>), typeof(GTGenericRepository<>))
	.AddScoped<IGTIdentityRepository, GTIdentityRepository>();

// Add BLL services
builder.Services
	.AddScoped<IGTListingService, GTListingService>()
	.AddScoped<IGTCompanyService, GTCompanyService>()
	.AddScoped<IGTLocationService, GTLocationService>()
	.AddScoped<IGTListingInquiryService, GTListingInquiryService>()
	.AddScoped<IGTExperienceLevelService, GTExperienceLevelService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
