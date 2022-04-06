using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTListingInquiryService : IGTListingInquiryService
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
			_logger = logger;
			_listingInquiryRepository = listingInquiryRepository;
			_userManager = userManager;
			_listingService = listingService;
		}

		public async Task<ListingInquiryDTO?> AddAsync(ListingInquiryDTO inquiryDTO, string? signedInUserId = null)
		{
			try
			{
				if (inquiryDTO is null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}
				if (inquiryDTO.ListingId is null || await _listingService.ExistsByIdAsync(inquiryDTO.ListingId))
				{
					_logger.LogWarning($"Attempted to add a listing inquiry without a listing id.");
					return null;
				}

				var newInquiry = new ListingInquiry();

				//TODO Implement automapper
				newInquiry.Id = inquiryDTO.Id;
				newInquiry.ApplicantId = inquiryDTO.ApplicantId;
				newInquiry.ListingId = inquiryDTO.ListingId;
				newInquiry.MessageTitle = inquiryDTO.MessageTitle;
				newInquiry.MessageBody = inquiryDTO.MessageBody;
				newInquiry.LinkedInLink = inquiryDTO.LinkedInLink;

				await _listingInquiryRepository.AddAsync(newInquiry);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				inquiryDTO.Id = newInquiry.Id;
				return inquiryDTO;
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
				if (_listingInquiryRepository.GetAll().Any(e => e.Id == id))
				{
					await _listingInquiryRepository.DeleteAsync(id);
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
					.GetAll()
					.AnyAsync(e => e.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<ListingInquiryDTO?>> GetAsync()
		{
			// Get entities from database
			try
			{
				var entities = await _listingInquiryRepository
					.GetAll()
					.ToListAsync();

				var inquiryDTOs = new List<ListingInquiryDTO>();

				foreach (var entity in entities)
				{

					var applicant = await _userManager.FindByIdAsync(entity.ApplicantId);

					// TODO add automapper
					
					inquiryDTOs.Add(new ListingInquiryDTO
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

		public async Task<ListingInquiryDTO?> GetByIdAsync(string id)
		{
			if (id is null)
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByIdAsync)}");
				return null;
			}

			try
			{
				var entity = await _listingInquiryRepository
					.GetAll()
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity == null)
				{
					_logger.LogInformation($"Entity with id {id} not found. {nameof(GetByIdAsync)}");
					return null;
				}

				var applicant = await _userManager
					.FindByIdAsync(entity.ApplicantId);

				// TODO Automapper

				var inquiryDTO = new ListingInquiryDTO()
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

		public async Task<List<ListingInquiryDTO>?> GetByListingIdAsync(string listingId)
		{
			if(listingId is null)
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByListingIdAsync)}");
				return null;
			}

			var entities = await _listingInquiryRepository
				.GetAll()
				.Where(e => e.ListingId == listingId)
				.ToListAsync();

			if(entities is null)
			{
				return null;
			}

			var listingInquiryDTOs = new List<ListingInquiryDTO>();

			foreach (var entity in entities)
			{
				var applicant = await _userManager
					.FindByIdAsync(entity.ApplicantId);

				listingInquiryDTOs.Add(new ListingInquiryDTO
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

		public async Task UpdateAsync(ListingInquiryDTO inquiryDTO, string id)
		{
			try
			{
				if (inquiryDTO.Id is not null && id is not null)
				{
					if (await ExistsByIdAsync(id))
					{
						var entityToUpdate = await _listingInquiryRepository
							.GetAll()
							.FirstOrDefaultAsync(e => e.Id == id);

						// TODO implement automapper
						entityToUpdate.Id = inquiryDTO.Id;
						entityToUpdate.ApplicantId = inquiryDTO.ApplicantId;
						entityToUpdate.ListingId = inquiryDTO.ListingId;
						entityToUpdate.LinkedInLink = inquiryDTO.LinkedInLink;
						entityToUpdate.MessageBody = inquiryDTO.MessageBody;
						entityToUpdate.MessageTitle = inquiryDTO.MessageTitle;

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
