using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Data.GTIdentityDb;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var identityConnectionString = builder.Configuration.GetConnectionString("GTIdentityContextConnection");
var appConnectionString = builder.Configuration.GetConnectionString("GTApplicationContextConnection");

builder.Services
	.AddDbContext<GTAppContext>(options =>
		options.UseSqlServer(appConnectionString))
	.AddDbContext<GTIdentityContext>(options =>
		options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
				options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<GTIdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DAL repositories
builder.Services
	.AddScoped(typeof(IGTGenericRepository<Location>), typeof(GTGenericRepository<Location>))
	.AddScoped(typeof(IGTGenericRepository<Company>), typeof(GTGenericRepository<Company>))
	.AddScoped(typeof(IGTGenericRepository<Listing>), typeof(GTGenericRepository<Listing>))
	.AddScoped(typeof(IGTGenericRepository<ListingInquiry>), typeof(GTGenericRepository<ListingInquiry>));

// Add BLL services
builder.Services
	.AddScoped<IGTLocationService, GTLocationService>()
	.AddScoped<IGTCompanyService, GTCompanyService>()
	.AddScoped<IGTListingService, GTListingService>()
	.AddScoped<IGTListingInquiryService, GTListingInquiryService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Adding Authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
				options =>
				{
					options.SaveToken = true;
					if (builder.Environment.IsDevelopment())
					{
						options.RequireHttpsMetadata = false;
					}

					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
					{
						AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidIssuer = configuration["JWT:ValidIssuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
					};
				})
		.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
				options => builder.Configuration.Bind("CookieSettings", options));

services.AddAuthorization(options =>
{
	// change the default policy to try out all existing authentication handlers
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.AddAuthenticationSchemes("Basic", JwtBearerDefaults.AuthenticationScheme)
			.Build();

	// use it later as [Authorize(Policy = "AdminPolicy")]
	options.AddPolicy("AdminPolicy", new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.AddAuthenticationSchemes("Basic", JwtBearerDefaults.AuthenticationScheme)
			.RequireClaim(ClaimTypes.Role, "GTAdministrator")
			.Build());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	GTAppDataSeeder.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
		name: "OnlyAction", // Route name
		pattern: "/{action}", // URL with parameters
		defaults: new { controller = "Home", action = "Index" }); // Parameter defaults

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
