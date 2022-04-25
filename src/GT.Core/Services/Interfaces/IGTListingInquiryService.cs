using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingInquiryService : IGTService
	{
		Task<List<ListingInquiryDTO?>> GetAllAsync();

		Task<ListingInquiryDTO?> GetByIdAsync(string id);

		Task<List<ListingInquiryDTO>?> GetByListingIdAsync(string id);

		Task<ListingInquiryDTO?> AddAsync(ListingInquiryDTO dto, string? signedInUserId = null);

		Task UpdateAsync(ListingInquiryDTO dto, string id);

		Task DeleteAsync(string id);

		Task DeleteInquiriesAssociatedWithUserIdAsync(string userId);

		Task<bool> ExistsByIdAsync(string id);
	}
}
