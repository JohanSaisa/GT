﻿using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTExperienceLevelService : IGTExperienceLevelService
	{
		private readonly ILogger<GTExperienceLevelService> _logger;
		private readonly IGTGenericRepository<ExperienceLevel> _experienceLevelRepository;

		public GTExperienceLevelService(ILogger<GTExperienceLevelService> logger,
			IGTGenericRepository<ExperienceLevel> experienceLevelRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_experienceLevelRepository = experienceLevelRepository
				?? throw new ArgumentNullException(nameof(experienceLevelRepository));
		}

		public async Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto)
		{
			try
			{
				if (dto is null || dto.Name == null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}

				if (await ExistsByNameAsync(dto.Name))
				{
					_logger.LogWarning($"Attempted to add a company whose name already exists in the database.");

					var entity = await _experienceLevelRepository.GetAll().Where(e => e.Name == dto.Name).FirstOrDefaultAsync();

					// TODO - Use IMapper
					if (entity is not null)
					{
						dto.Id = entity.Id;
						dto.Name = entity.Name;
					}

					return dto;
				}

				// TODO - Use IMapper
				var newEntity = new ExperienceLevel()
				{
					Id = Guid.NewGuid().ToString(),
					Name = dto.Name
				};

				await _experienceLevelRepository.AddAsync(newEntity);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				dto.Id = newEntity.Id;

				return dto;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			try
			{
				return await _experienceLevelRepository.GetAll().Where(e => e.Name == name).AnyAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<ExperienceLevelDTO?>> GetAllAsync()
		{
			try
			{
				var experienceLevelEntitiess = await _experienceLevelRepository
					.GetAll()
					.ToListAsync();

				var experienceLevelDTOs = new List<ExperienceLevelDTO>();

				//TODO automapper
				foreach (var entity in experienceLevelEntitiess)
				{
					experienceLevelDTOs.Add(new ExperienceLevelDTO()
					{
						Id = entity.Id,
						Name = entity.Name
					});
				}

				return experienceLevelDTOs;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task UpdateAsync(ExperienceLevelDTO experienceLevelDTO, string name)
		{
			try
			{
				if (experienceLevelDTO.Name != name)
				{
					_logger.LogWarning($"Names are not matching in method: {nameof(UpdateAsync)}.");
					return;
				}
				if (experienceLevelDTO.Id is not null && name is not null)
				{
					if (await ExistsByNameAsync(name))
					{
						var entityToUpdate = await _experienceLevelRepository.GetAll().FirstOrDefaultAsync(e => e.Id == experienceLevelDTO.Id);

						// TODO implement automapper
						

						await _experienceLevelRepository.UpdateAsync(entityToUpdate, entityToUpdate.Id);
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
