using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTAppDb.Entities
{
	internal class ListingInquiry : IGTEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }
		[Column(TypeName = "nvarchar(100)")]
		public string MessageTitle { get; set; }
		[Column(TypeName = "nvarchar(500)")]
		public string MessageBody { get; set; }
		[Column(TypeName = "nvarchar(100)")]
		public string? LinkedInLink { get; set; }
		[Column(TypeName = "nvarchar(450)")]
		public string? ApplicantId { get; set; }
		public Listing Listing { get; set; }
	}
}