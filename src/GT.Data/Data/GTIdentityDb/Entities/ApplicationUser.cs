using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTIdentityDb.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser, IGTEntity
{
	[PersonalData]
	[Column(TypeName = "nvarchar(70)")]
	public string? FirstName { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(70)")]
	public string? LastName { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(254)")]
	public string? LinkedInURL { get; set; }
}
