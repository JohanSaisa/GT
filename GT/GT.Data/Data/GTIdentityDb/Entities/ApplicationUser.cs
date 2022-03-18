using Microsoft.AspNetCore.Identity;

namespace GT.Data.Data.GTIdentityDb.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser, IGTEntity
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}