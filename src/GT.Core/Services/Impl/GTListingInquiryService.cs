using GT.Core.DTO.Inquiry;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTListingInquiryService : IGTInquiryService
	{
		private readonly ILogger<GTListingInquiryService> _logger;
		private readonly IGTGenericRepository<ListingInquiry> _listingInquiryRepository;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IGTListingService _listingService;

		public GTListingInquiryService(ILogger<GTListingInquiryService> logger,
			IGTGenericRepository<ListingInquiry> listingInquiryRepository,
			UserManager<ApplicationUser> userManager,
			IGTListingService listingService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_listingInquiryRepository = listingInquiryRepository
				?? throw new ArgumentNullException(nameof(listingInquiryRepository));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
		}

		public async Task<InquiryDTO?> AddAsync(InquiryDTO dto, string? signedInUserId = null)
		{
			try
			{
				if (dto is null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}
				if (dto.ListingId is null || !(await _listingService.ExistsByIdAsync(dto.ListingId)))
				{
					_logger.LogWarning($"Attempted to add a listing inquiry without a listing id.");
					return null;
				}

				if (dto.ApplicantId != null)
				{
					if (_listingInquiryRepository.Get().Any(e => e.ApplicantId == dto.ApplicantId && e.ListingId == dto.ListingId))
					{
						_logger.LogWarning($"Attempted to add another listing inquiry by same user id.");
						return null;
					}
				}

				var newInquiry = new ListingInquiry();

				//TODO Implement automapper
				newInquiry.Id = Guid.NewGuid().ToString();
				newInquiry.ApplicantId = signedInUserId;
				newInquiry.ListingId = dto.ListingId;
				newInquiry.MessageTitle = dto.MessageTitle;
				newInquiry.MessageBody = dto.MessageBody;
				newInquiry.LinkedInLink = dto.LinkedInLink;

				await _listingInquiryRepository.AddAsync(newInquiry);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				dto.Id = newInquiry.Id;
				return dto;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task DeleteAsync(string id)
		{
			try
			{
				var entity = await _listingInquiryRepository.Get()
					.Include(e => e.Listing)
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity is not null)
				{
					await _listingInquiryRepository.DeleteAsync(entity);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task DeleteInquiriesAssociatedWithUserIdAsync(string userId)
		{
			try
			{
				var associatedInquiries = await _listingInquiryRepository.Get()
					.Where(e => e.ApplicantId == userId)
					.ToListAsync();

				if (associatedInquiries is not null)
				{
					foreach (var entity in associatedInquiries)
					{
						await _listingInquiryRepository.DeleteAsync(entity);
					}
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<bool> ExistsByIdAsync(string id)
		{
			try
			{
				return await _listingInquiryRepository
					.Get()
					.AnyAsync(e => e.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<InquiryDTO?>> GetAllAsync()
		{
			// Get entities from database
			try
			{
				var entities = await _listingInquiryRepository
					.Get()
					.ToListAsync();

				var inquiryDTOs = new List<InquiryDTO>();

				foreach (var entity in entities)
				{
					var applicant = await _userManager.FindByIdAsync(entity.ApplicantId);

					// TODO add automapper

					inquiryDTOs.Add(new InquiryDTO
					{
						Id = entity.Id,
						ApplicantId = entity.ApplicantId,
						ApplicantEmail = applicant?.Email,
						ListingId = entity.ListingId,
						LinkedInLink = entity.LinkedInLink,
						MessageBody = entity.MessageBody,
						MessageTitle = entity.MessageTitle
					});
				}

				return inquiryDTOs;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task<InquiryDTO?> GetByIdAsync(string id)
		{
			if (id is null)
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByIdAsync)}");
				return null;
			}

			try
			{
				var entity = await _listingInquiryRepository
					.Get()!
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity == null)
				{
					_logger.LogInformation($"Entity with id {id} not found. {nameof(GetByIdAsync)}");
					return null;
				}

				var applicant = await _userManager
					.FindByIdAsync(entity.ApplicantId);

				// TODO Automapper

				var inquiryDTO = new InquiryDTO()
				{
					Id = entity.Id,
					ApplicantId = entity.ApplicantId,
					ApplicantEmail = applicant?.Email,
					ListingId = entity.ListingId,
					LinkedInLink = entity.LinkedInLink,
					MessageBody = entity.MessageBody,
					MessageTitle = entity.MessageTitle
				};

				return inquiryDTO;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task<List<InquiryDTO>?> GetByListingIdAsync(string id)
		{
			if (id is null)
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByListingIdAsync)}");
				return null;
			}

			var entities = await _listingInquiryRepository
				.Get()
				.Where(e => e.ListingId == id)
				.ToListAsync();

			if (entities is null)
			{
				return null;
			}

			var listingInquiryDTOs = new List<InquiryDTO>();

			foreach (var entity in entities)
			{
				var applicant = await _userManager
					.FindByIdAsync(entity.ApplicantId);

				listingInquiryDTOs.Add(new InquiryDTO
				{
					Id = entity.Id,
					ApplicantId = entity.ApplicantId,
					ApplicantEmail = applicant?.Email,
					ListingId = entity.ListingId,
					LinkedInLink = entity.LinkedInLink,
					MessageBody = entity.MessageBody,
					MessageTitle = entity.MessageTitle
				});
			}

			return listingInquiryDTOs;
		}

		public async Task UpdateAsync(InquiryDTO dto, string id)
		{
			try
			{
				if (dto.Id != id)
				{
					_logger.LogWarning($"IDs are not matching in method: {nameof(UpdateAsync)}.");
					return;
				}
				if (dto.Id is not null && id is not null)
				{
					if (await ExistsByIdAsync(id))
					{
						var entityToUpdate = await _listingInquiryRepository
							.Get()
							.FirstOrDefaultAsync(e => e.Id == id);

						// TODO implement automapper
						entityToUpdate.Id = dto.Id;
						entityToUpdate.ApplicantId = dto.ApplicantId;
						entityToUpdate.ListingId = dto.ListingId;
						entityToUpdate.LinkedInLink = dto.LinkedInLink;
						entityToUpdate.MessageBody = dto.MessageBody;
						entityToUpdate.MessageTitle = dto.MessageTitle;

						await _listingInquiryRepository.UpdateAsync(entityToUpdate, id);
					}
				}
				else
				{
					_logger.LogWarning($"Arguments cannot be null when using the method: {nameof(UpdateAsync)}.");
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}
	}
}
