using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Inquiry
{
	public class PatchInquiryDTO
	{
		[Required(ErrorMessage = "MessageTitle is required")]
		[StringLength(100)]
		public string? MessageTitle { get; set; }

		[Required(ErrorMessage = "MessageBody is required")]
		[StringLength(500)]
		public string? MessageBody { get; set; }
	}
}
