using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTLocationService : IGTLocationService
	{
		private readonly ILogger<GTLocationService> _logger;
		private readonly IGTGenericRepository<Location> _locationRepository;

		public GTLocationService(ILogger<GTLocationService> logger,
			IGTGenericRepository<Location> locationRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
		}
		
		public async Task<LocationDTO> AddAsync(LocationDTO dto)
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
					var entity = await _locationRepository
						.GetAll()
						.FirstOrDefaultAsync(e => e.Name == dto.Name);

					// TODO - Use IMapper
					if (entity is not null)
					{
						dto.Id = entity.Id;
						dto.Name = entity.Name;
					}

					return dto;
				}

				// TODO - Use IMapper
				var newEntity = new Location()
				{
					Id = Guid.NewGuid().ToString(),
					Name = dto.Name
				};

				await _locationRepository.AddAsync(newEntity);

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

		public Task UpdateAsync(LocationDTO dto, string id)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			try
			{
				return await _locationRepository
					.GetAll()
					.AnyAsync(e => e.Name == name);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<LocationDTO>?> GetAllAsync()
		{
			try
			{
				var query = _locationRepository
					.GetAll();

				return await query
					.Select(e => new LocationDTO
					{
						Id = e.Id,
						Name = e.Name
					})
					.ToListAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}
		
		public Task<LocationDTO?> GetByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

	}
}
