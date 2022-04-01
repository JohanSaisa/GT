using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTListingInquiryService : IGTListingInquiryService
	{
		private readonly ILogger<GTListingInquiryService> _logger;
		private readonly IGTGenericRepository<ListingInquiry> _listingInquiryRepository;

		public GTListingInquiryService(ILogger<GTListingInquiryService> logger,
			IGTGenericRepository<ListingInquiry> listingInquiryRepository)
		{
			_logger = logger;
			_listingInquiryRepository = listingInquiryRepository;
		}

		public Task<ListingInquiryDTO?> AddAsync(ListingInquiryDTO inquiryDTO, string? signedInUserId = null)
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

		public Task<List<ListingInquiryDTO?>> GetAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ListingInquiryDTO?> GetByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(ListingInquiryDTO inquiryDTO, string id)
		{
			throw new NotImplementedException();
		}
	}
}
