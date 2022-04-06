using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingInquiryService : IGTService
	{
		Task<List<ListingInquiryDTO?>> GetAsync();

		Task<ListingInquiryDTO?> GetByIdAsync(string id);

		Task<List<ListingInquiryDTO>?> GetByListingIdAsync(string listingId);

		Task<ListingInquiryDTO?> AddAsync(ListingInquiryDTO inquiryDTO, string? signedInUserId = null);

		Task UpdateAsync(ListingInquiryDTO inquiryDTO, string id);

		Task DeleteAsync(string id);

		Task<bool> ExistsByIdAsync(string id);
	}
}
