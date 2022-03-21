using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTIdentityDb.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser, IGTEntity
{
	[Column(TypeName = "nvarchar(70)")]
	public string? FirstName { get; set; }
	[Column(TypeName = "nvarchar(70)")]
	public string? LastName { get; set; }
}