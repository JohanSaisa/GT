using AutoMapper;
using AutoMapper.QueryableExtensions;
using GT.Core.DTO.Inquiry;
using GT.Core.Services.Interfaces;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Data.IdentityDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class InquiryService : IInquiryService
	{
		private readonly IMapper _mapper;
		private readonly IGenericRepository<Inquiry> _inquiryRepository;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IListingService _listingService;

		public InquiryService(IMapper mapper,
			IGenericRepository<Inquiry> inquiryRepository,
			UserManager<ApplicationUser> userManager,
			IListingService listingService)
		{
			_mapper = mapper;
			_inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
		}

		public async Task<bool> AddAsync(InquiryDTO dto)
		{
			if (await _inquiryRepository.Get()!.AnyAsync(e => e.ApplicantEmail == dto.ApplicantEmail && e.ListingId == dto.ListingId))
			{
				throw new ArgumentException($"An inquiry for this listing has already been made with email {dto.ApplicantEmail}.");
			}

			var newInquiry = new Inquiry
			{
				Id = Guid.NewGuid().ToString(),
				ApplicantEmail = dto.ApplicantEmail,
				ListingId = dto.ListingId,
				MessageTitle = dto.MessageTitle,
				MessageBody = dto.MessageBody,
				LinkedInLink = dto.LinkedInLink
			};

			await _inquiryRepository.AddAsync(newInquiry);

			return await _inquiryRepository.SaveAsync();
		}

		public async Task<List<InquiryDTO>> GetAllAsync()
		{
			var inquiryDTOs = await _inquiryRepository
				.Get()!
				.ProjectTo<InquiryDTO>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return inquiryDTOs;
		}

		public async Task<InquiryDTO?> GetByIdAsync(string id)
		{
			var inquiryDTO = await _inquiryRepository
				.Get()!
				.ProjectTo<InquiryDTO>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(e => e.Id == id);

			return inquiryDTO;
		}

		public async Task<bool> ExistsByIdAsync(string id)
		{
			return await _inquiryRepository
				.Get()!
				.AnyAsync(e => e.Id == id);
		}

		public async Task DeleteAsync(string id)
		{
			var entity = await _inquiryRepository
				.Get()!
				.Include(e => e.Listing)
				.FirstOrDefaultAsync(e => e.Id == id);

			_inquiryRepository.Delete(entity);

			if (!await _inquiryRepository.SaveAsync())
			{
				throw new Exception($"Could not delete inquiry with id '{id}' in database.");
			}
		}

		public async Task UpdateAsync(PostInquiryDTO dto, string id)
		{
			var entityToUpdate = await _inquiryRepository
				.Get()!
				.FirstOrDefaultAsync(e => e.Id == id);
			if (entityToUpdate is null)
			{
				throw new ArgumentException($"No Inquiry with id '{id}' was found.");
			}

			entityToUpdate.ApplicantEmail = dto.ApplicantEmail;
			entityToUpdate.ListingId = dto.ListingId;
			entityToUpdate.LinkedInLink = dto.LinkedInLink;
			entityToUpdate.MessageBody = dto.MessageBody;
			entityToUpdate.MessageTitle = dto.MessageTitle;

			_inquiryRepository.Update(entityToUpdate);

			if (!await _inquiryRepository.SaveAsync())
			{
				throw new Exception($"Could not update inquiry with id '{id}' in database.");
			}
		}

		public async Task DeleteInquiriesAssociatedWithUserIdAsync(string userEmail)
		{
			var associatedInquiries = await _inquiryRepository
				.Get()!
				.Where(e => e.ApplicantEmail == userEmail)
				.ToListAsync();

			foreach (var entity in associatedInquiries)
			{
				_inquiryRepository.Delete(entity);
			}

			if (await _inquiryRepository.SaveAsync())
			{
				throw new Exception($"Could not delete associated inquiries with email '{userEmail}' in database.");
			}
		}

		public async Task<List<InquiryDTO>> GetByListingIdAsync(string id)
		{
			var inquiryDTOs = await _inquiryRepository
				.Get()!
				.Where(e => e.ListingId == id)
				.ProjectTo<InquiryDTO>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return inquiryDTOs;
		}
	}
}
