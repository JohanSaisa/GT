using GT.Core.DTO.ExperienceLevel;
using GT.Core.Services.Interfaces;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class ExperienceLevelService : IExperienceLevelService
	{
		private readonly ILogger<ExperienceLevelService> _logger;
		private readonly IGenericRepository<ExperienceLevel> _experienceLevelRepository;

		public ExperienceLevelService(ILogger<ExperienceLevelService> logger,
			IGenericRepository<ExperienceLevel> experienceLevelRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_experienceLevelRepository = experienceLevelRepository
				?? throw new ArgumentNullException(nameof(experienceLevelRepository));
		}

		public async Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto)
		{
			try
			{
				if (dto is null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}

				if (String.IsNullOrWhiteSpace(dto.Name))
				{
					_logger.LogWarning($"Attempted to add an ExperienceLevel without a name to the database.");
					return null;
				}

				dto.Name = dto.Name.Trim();

				if (await ExistsByNameAsync(dto.Name))
				{
					_logger.LogWarning($"Attempted to add a company whose name already exists in the database.");

					var entity = await _experienceLevelRepository.Get().Where(e => e.Name == dto.Name).FirstOrDefaultAsync();

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

		public async Task DeleteAsync(string id)
		{
			try
			{
				var entity = await _experienceLevelRepository.Get()
					.Include(e => e.Listings)
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity is not null)
				{
					await _experienceLevelRepository.DeleteAsync(entity);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			try
			{
				return await _experienceLevelRepository.Get().AnyAsync(e => e.Name == name);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<bool> ExistsByIdAsync(string id)
		{
			try
			{
				return await _experienceLevelRepository.Get().AnyAsync(e => e.Id == id);
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
					.Get()
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

		public async Task<ExperienceLevelDTO?> GetByIdAsync(string id)
		{
			try
			{
				// Get entity
				var entity = await _experienceLevelRepository
					.Get()
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity == null)
				{
					_logger.LogInformation($"Entity with id {id} not found.");
					return null;
				}

				// Map entity to DTO
				var experienceLevelDTO = new ExperienceLevelDTO()
				{
					Id = entity.Id,
					Name = entity.Name,
				};

				return experienceLevelDTO;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task UpdateAsync(ExperienceLevelDTO experienceLevelDTO, string id)
		{
			try
			{
				if (experienceLevelDTO.Id != id)
				{
					_logger.LogWarning($"Ids are not matching in method: {nameof(UpdateAsync)}.");
					return;
				}
				if (experienceLevelDTO.Id is not null && id is not null)
				{
					if (await ExistsByIdAsync(id))
					{
						var entityToUpdate = await _experienceLevelRepository.Get().FirstOrDefaultAsync(e => e.Id == experienceLevelDTO.Id);

						// TODO: Refactor and implement automapper
						if (entityToUpdate is null)
						{
							_logger.LogWarning($"Entity was not found with existing Id");
							return;
						}

						await _experienceLevelRepository.UpdateAsync(entityToUpdate, id);
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
