using GT.Core.DTO.Company;
using GT.Core.DTO.ExperienceLevel;
using GT.Core.DTO.Impl;
using GT.Core.DTO.Listing;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GT.API.Controllers
{
	[Route("api/v1/listings")]
	[ApiController]
	public class ListingController : GTControllerBase
	{
		private readonly IListingService _listingService;
		private readonly ICompanyService _companyService;
		private readonly IExperienceLevelService _experienceLevelService;
		private readonly ILocationService _locationService;

		public ListingController(
			IListingService listingService,
			ICompanyService companyService,
			IExperienceLevelService experienceLevelService,
			ILocationService locationService,
			IConfiguration configuration) : base(configuration)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
			_experienceLevelService = experienceLevelService ?? throw new ArgumentNullException(nameof(experienceLevelService));
			_locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
		}

		[HttpPost("overview")]
		public async Task<ActionResult<List<ListingOverviewDTO>>> GetListingsWithFilter(PostListingFilterDTO? filterModel)
		{
			if (filterModel is null)
			{
				filterModel = new PostListingFilterDTO();
			}

			try
			{
				var dtos = await _listingService.GetAllByFilterAsync(filterModel);
				return Ok(dtos);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ListingDTO>> GetListing(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				var dto = await _listingService.GetByIdAsync(id);

				if (dto is null)
				{
					return NotFound();
				}

				return Ok(dto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			if (!await _listingService.ExistsByIdAsync(id))
			{
				return NotFound();
			}

			try
			{
				await _listingService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, PostListingDTO dto)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				// Add Location if it does not exist
				if (dto.Location is not null && !await _locationService.ExistsByNameAsync(dto.Location.Trim()))
				{
					await _locationService.AddAsync(new PostLocationDTO { Name = dto.Location.Trim() });
				}

				// Add Company if it does not exist
				if (dto.Employer is not null && !await _companyService.ExistsByNameAsync(dto.Employer.Trim()))
				{
					await _companyService.AddAsync(new PostCompanyDTO { Name = dto.Employer.Trim() });
				}

				// Add ExperienceLevel if it does not exist
				if (dto.ExperienceLevel is not null && !await _experienceLevelService.ExistsByNameAsync(dto.ExperienceLevel))
				{
					await _experienceLevelService.AddAsync(new PostExperienceLevelDTO { Name = dto.ExperienceLevel.Trim() });
				}

				await _listingService.UpdateAsync(dto, id);

				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult> PostListing(PostListingDTO dto)
		{
			try
			{
				// Add Location if it does not exist
				if (dto.Location is not null && !await _locationService.ExistsByNameAsync(dto.Location.Trim()))
				{
					await _locationService.AddAsync(new PostLocationDTO { Name = dto.Location.Trim() });
				}

				// Add Company if it does not exist
				if (dto.Employer is not null && !await _companyService.ExistsByNameAsync(dto.Employer.Trim()))
				{
					await _companyService.AddAsync(new PostCompanyDTO { Name = dto.Employer.Trim() });
				}

				// Add ExperienceLevel if it does not exist
				if (dto.ExperienceLevel is not null && !await _experienceLevelService.ExistsByNameAsync(dto.ExperienceLevel))
				{
					await _experienceLevelService.AddAsync(new PostExperienceLevelDTO { Name = dto.ExperienceLevel.Trim() });
				}

				await _listingService.AddAsync(dto, GetUserId());

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
