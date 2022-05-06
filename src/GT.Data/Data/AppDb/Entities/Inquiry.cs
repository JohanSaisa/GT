using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.AppDb.Entities
{
	public class Inquiry : IAppEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(100)")]
		public string? MessageTitle { get; set; }

		[Column(TypeName = "nvarchar(500)")]
		public string? MessageBody { get; set; }

		[Column(TypeName = "nvarchar(254)")]
		public string? LinkedInLink { get; set; }

		[Column(TypeName = "nvarchar(256)")]
		public string? ApplicantEmail { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? ListingId { get; set; }

		public Listing? Listing { get; set; }
	}
}
