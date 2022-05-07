using GT.Core.DTO.Inquiry;

namespace GT.Core.Services.Interfaces
{
	public interface IInquiryService
	{
		Task<bool> AddAsync(PostInquiryDTO dto, string? signedInUserId = null);

		Task<List<InquiryDTO>> GetAllAsync();

		Task<InquiryDTO?> GetByIdAsync(string id);

		Task<bool> ExistsByIdAsync(string id);

		Task DeleteAsync(string id);

		Task UpdateAsync(PostInquiryDTO dto, string id);

		Task DeleteInquiriesAssociatedWithUserIdAsync(string userId);

		Task<List<InquiryDTO>> GetByListingIdAsync(string id);
	}
}
