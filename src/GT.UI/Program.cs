using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Data.GTIdentityDb;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var identityConnectionString = builder.Configuration.GetConnectionString("GTIdentityContextConnection");
var appConnectionString = builder.Configuration.GetConnectionString("GTApplicationContextConnection");

builder.Services
	.AddDbContext<GTAppContext>(options =>
		options.UseSqlServer(appConnectionString))
	.AddDbContext<GTIdentityContext>(options =>
		options.UseSqlServer(identityConnectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
		 options.SignIn.RequireConfirmedAccount = true)
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
	.AddScoped<IGTService, GTListingService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
