using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Inquiry
{
	/// <summary>
	/// Represents a ListingInquiry view and create model.
	/// </summary>
	public class PatchInquiryDTO
	{
		[Required]
		[StringLength(100)]
		public string? MessageTitle { get; set; }

		[Required]
		[StringLength(500)]
		public string? MessageBody { get; set; }
	}
}
