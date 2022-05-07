using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Inquiry
{
	public class PostInquiryDTO
	{
		[Required(ErrorMessage = "ListingId is required")]
		[StringLength(450)]
		public string? ListingId { get; set; }

		[Required(ErrorMessage = "ApplicantEmail is required")]
		[StringLength(254)]
		public string? ApplicantEmail { get; set; }

		[Required(ErrorMessage = "MessageTitle is required")]
		[StringLength(100)]
		public string? MessageTitle { get; set; }

		[Required(ErrorMessage = "MessageBody is required")]
		[StringLength(500)]
		public string? MessageBody { get; set; }

		[StringLength(254)]
		public string? LinkedInLink { get; set; }
	}
}
