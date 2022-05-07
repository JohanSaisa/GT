using AutoMapper;
using AutoMapper.QueryableExtensions;
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
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ExperienceLevel> _experienceLevelRepository;

		public ExperienceLevelService(IMapper mapper,
			IGenericRepository<ExperienceLevel> experienceLevelRepository)
		{
			_mapper = mapper;
			_experienceLevelRepository = experienceLevelRepository
				?? throw new ArgumentNullException(nameof(experienceLevelRepository));
		}

		public async Task<bool> AddAsync(PostExperienceLevelDTO dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				throw new ArgumentException("Name cannot be null or empty.");
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

		public async Task<List<ExperienceLevelDTO>> GetAllAsync()
		{
			var experienceLevelDTOs = await _experienceLevelRepository
				.Get()!
				.ProjectTo<ExperienceLevelDTO>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return experienceLevelDTOs;
		}

		public async Task<ExperienceLevelDTO?> GetByIdAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be null or empty.");
			}

			var dto = await _experienceLevelRepository
				.Get()!
				.ProjectTo<ExperienceLevelDTO>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync(e => e.Id == id);

			return dto;
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name cannot be null or empty.");
			}

			return await _experienceLevelRepository.Get()!.AnyAsync(e => e.Name == name);
		}

		public async Task<bool> DeleteAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be null or empty.");
			}

			var entity = await _experienceLevelRepository.Get()!
				.Include(e => e.Listings)
				.SingleOrDefaultAsync(e => e.Id == id);

			if (entity is null)
			{
				throw new ArgumentException($"No ExperienceLevel with id '{id}' was found.");
			}

			_experienceLevelRepository.Delete(entity);

			return await _experienceLevelRepository.SaveAsync();
		}

		public async Task<bool> UpdateAsync(PostExperienceLevelDTO dto, string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be null or empty.");
			}

			if (dto is null)
			{
				throw new ArgumentNullException("ExperienceLevel object cannot be null.");
			}

			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				throw new ArgumentNullException($"Name cannot be null or empty.");
			}

			dto.Name = dto.Name.Trim();

			if (await _experienceLevelRepository.Get()!.AnyAsync((e => e.Id != id && e.Name == dto.Name)))
			{
				throw new ArgumentException($"ExperienceLevel with name: {dto.Name} already exist.");
			}

			var entityToUpdate = await _experienceLevelRepository.Get()!.SingleOrDefaultAsync(e => e.Id == id);

			if (entityToUpdate is null)
			{
				throw new Exception($"No ExperienceLevel with id '{id}' was found.");
			}

			entityToUpdate.Name = dto.Name;

			_experienceLevelRepository.Update(entityToUpdate);

			return await _experienceLevelRepository.SaveAsync();
		}
	}
}
