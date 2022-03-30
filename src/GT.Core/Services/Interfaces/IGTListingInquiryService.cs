using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingInquiryService : IGTService
	{
		Task<List<ListingInquiryDTO>> GetAsync(IListingFilterModel? filter = null);
		Task<ListingInquiryDTO> GetByIdAsync(string id);
		Task<ListingInquiryDTO> AddAsync(ListingInquiryDTO inquiryDTO, string? signedInUserId = null);
		Task UpdateAsync(ListingInquiryDTO inquiryDTO, string id);
		Task DeleteAsync(string id);
		Task<bool> ExistsByIdAsync(string id);
	}
}
