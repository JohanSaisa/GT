using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.IdentityDb.Entities;

public class ApplicationUser : IdentityUser, IAppEntity
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
