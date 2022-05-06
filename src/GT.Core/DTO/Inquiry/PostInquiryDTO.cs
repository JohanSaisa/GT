using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Inquiry
{
	/// <summary>
	/// Represents a ListingInquiry view and create model.
	/// </summary>
	public class PostInquiryDTO
	{
		[Required]
		[StringLength(450)]
		public string? ListingId { get; set; }

		[Required]
		[StringLength(254)]
		public string? ApplicantEmail { get; set; }

		[Required]
		[StringLength(100)]
		public string? MessageTitle { get; set; }

		[Required]
		[StringLength(500)]
		public string? MessageBody { get; set; }

		[Required]
		[StringLength(254)]
		public string? LinkedInLink { get; set; }
	}
}
