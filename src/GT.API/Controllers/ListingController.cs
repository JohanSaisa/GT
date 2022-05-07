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
				var dtos = await _listingService
					.GetAllByFilterAsync(filterModel);

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
				var dto = await _listingService
					.GetByIdAsync(id);

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
				if (dto.Employer is not null && !await _companyService.ExistsByNameAsync(dto.Employer.Trim()))
				{
					await _companyService.AddAsync(new PostCompanyDTO { Name = dto.Employer.Trim() });
				}

				if (dto.Location is not null && !await _locationService.ExistsByNameAsync(dto.Location.Trim()))
				{
					await _locationService.AddAsync(new PostLocationDTO { Name = dto.Location.Trim() });
				}

				if (dto.ExperienceLevel is not null && !await _experienceLevelService.ExistsByNameAsync(dto.ExperienceLevel))
				{
					await _experienceLevelService.AddAsync(new PostExperienceLevelDTO { Name = dto.ExperienceLevel.Trim() });
				}

				await _listingService.UpdateAsync(dto, id);

				return Ok();
			}
			catch
			{
				return StatusCode(500);
			}
		}

		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string?>> PostListing(ListingDTO dto)
		{
			if (dto is null)
			{
				return BadRequest();
			}

			try
			{
				// Ensures that all necessary entities exists in the database.
				await _companyService.AddAsync(new CompanyDTO { Name = dto.Employer });
				await _locationService.AddAsync(new LocationDTO { Name = dto.Location });
				await _experienceLevelService.AddAsync(new ExperienceLevelDTO { Name = dto.Location });

				var objToReturn = await _listingService.AddAsync(dto, GetUserId());
				var result = JsonConvert.SerializeObject(objToReturn);
				return StatusCode(201, result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}
