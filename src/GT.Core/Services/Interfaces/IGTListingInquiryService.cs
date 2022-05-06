using GT.Core.DTO.Inquiry;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingInquiryService : IGTService
	{
		Task<List<InquiryDTO?>> GetAllAsync();

		Task<InquiryDTO?> GetByIdAsync(string id);

		Task<List<InquiryDTO>?> GetByListingIdAsync(string id);

		Task<InquiryDTO?> AddAsync(InquiryDTO dto, string? signedInUserId = null);

		Task UpdateAsync(InquiryDTO dto, string id);

		Task DeleteAsync(string id);

		Task DeleteInquiriesAssociatedWithUserIdAsync(string userId);

		Task<bool> ExistsByIdAsync(string id);
	}
}
