using GT.Core.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	/// <summary>
	/// Represents a ListingInquiry view and create model.
	/// </summary>
	public class ListingInquiryDTO : IGTDataTransferObject
	{
		[StringLength(450)]
		public string Id { get; set; }

		[StringLength(450)]
		public string ListingId { get; set; }

		[StringLength(450)]
		public string? ApplicantId { get; set; }

		[StringLength(100)]
		public string MessageTitle { get; set; }

		[StringLength(500)]
		public string MessageBody { get; set; }

		[StringLength(100)]
		public string LinkedInLink { get; set; }
	}
}
