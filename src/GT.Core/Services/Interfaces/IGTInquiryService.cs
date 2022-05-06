using GT.Core.DTO.Inquiry;

namespace GT.Core.Services.Interfaces
{
	public interface IGTInquiryService
	{
		Task<bool> AddAsync(PostInquiryDTO dto, string? signedInUserId = null);

		Task<List<InquiryDTO>> GetAllAsync();

		Task<InquiryDTO?> GetByIdAsync(string id);

		Task<bool> ExistsByIdAsync(string id);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostInquiryDTO dto, string id);

		Task<bool> DeleteInquiriesAssociatedWithUserIdAsync(string userId);

		Task<List<InquiryDTO>> GetByListingIdAsync(string id);
	}
}
