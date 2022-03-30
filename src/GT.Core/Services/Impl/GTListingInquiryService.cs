using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;

namespace GT.Core.Services.Impl
{
	public class GTListingInquiryService : IGTListingInquiryService
	{
		public Task<ListingInquiryDTO> AddAsync(ListingInquiryDTO inquiryDTO, string? signedInUserId = null)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ExistsByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<List<ListingInquiryDTO>> GetAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ListingInquiryDTO> GetByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(ListingInquiryDTO inquiryDTO, string id)
		{
			throw new NotImplementedException();
		}
	}
}
