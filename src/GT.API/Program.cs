using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTIdentityDb;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories.Impl;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
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
	.AddScoped<IGTInquiryService, GTListingInquiryService>()
	.AddScoped<IGTExperienceLevelService, GTExperienceLevelService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Authentication.
builder.Services.AddAuthentication()
	.AddCookie(cfg => cfg.SlidingExpiration = true)
	.AddJwtBearer(cfg =>
	{
		cfg.SaveToken = true;
		if (builder.Environment.IsDevelopment())
		{
			cfg.RequireHttpsMetadata = false;
		}
		cfg.TokenValidationParameters = new TokenValidationParameters()
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
			ValidIssuer = configuration["Authentication:Jwt:Issuer"],
			ValidAudience = configuration["Authentication:Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"])),
		};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	GTAppDataSeeder.Initialize(services);
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
