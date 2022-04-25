using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingInquiryService : 
		IGTService<ListingInquiryDTO>,
		IHasGetAllAsyncWithoutParameters<ListingInquiryDTO>,
		IHasAddAsyncWithUserId<ListingInquiryDTO>
	{
		Task<List<ListingInquiryDTO>?> GetByListingIdAsync(string id);

		Task DeleteInquiriesAssociatedWithUserIdAsync(string userId);
	}
}
