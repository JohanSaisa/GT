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
		private readonly IGenericRepository<ExperienceLevel> _experienceLevelRepository;

		public ExperienceLevelService(IGenericRepository<ExperienceLevel> experienceLevelRepository)
		{
			_experienceLevelRepository = experienceLevelRepository
				?? throw new ArgumentNullException(nameof(experienceLevelRepository));
		}

		public async Task<bool> AddAsync(PostExperienceLevelDTO dto)
		{
			if (string.IsNullOrEmpty(dto.Name!.Trim()))
			{
				throw new ArgumentException("Name property cannot be null or empty.");
			}

			dto.Name = dto.Name.Trim();

			
			if (await ExistsByNameAsync(dto.Name))
			{
				throw new ArgumentException($"ExperienceLevel with name {dto.Name} already exists.");
			}

			var entity = new ExperienceLevel
			{
				Id = Guid.NewGuid().ToString(),
				Name = dto.Name
			};

			await _experienceLevelRepository.AddAsync(entity);

			return await _experienceLevelRepository.SaveAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var entity = await _experienceLevelRepository.Get()!
				.Include(e => e.Listings)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (entity is null)
			{
				
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
