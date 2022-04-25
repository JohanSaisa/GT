using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GT.API.Controllers
{
	[Route("api/v1/listings")]
	[ApiController]
	public class ListingController : ControllerBase
	{
		private readonly IGTListingService _listingService;
		private readonly IGTCompanyService _companyService;
		private readonly IGTExperienceLevelService _experienceLevelService;
		private readonly IGTLocationService _locationService;
		private readonly IConfiguration _configuration;

		public ListingController(
			IGTListingService listingService,
			IGTCompanyService companyService,
			IGTExperienceLevelService experienceLevelService,
			IGTLocationService locationService,
			IGTListingInquiryService listingInquiryService,
			IConfiguration configuration)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
			_experienceLevelService = experienceLevelService ?? throw new ArgumentNullException(nameof(experienceLevelService));
			_locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
			_configuration = configuration;
		}

		// GET: /overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetListingsWithFilter()
		{
			ListingFilterModel filterModel = new ListingFilterModel();

			var dtos = await _listingService
				.GetAllByFilterAsync(filterModel);

			if (dtos == null)
			{
				return NotFound();
			}
			var result = JsonConvert.SerializeObject(dtos);

			return Ok(result);
		}

		// GET:/5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetListing(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var dto = await _listingService
				.GetByIdAsync(id);

			if (dto == null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(dto);

			return Ok(result);
		}

		// GET: delete/5
		[Route("delete")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string? id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				await _listingService.DeleteAsync(id);
			}
			catch (Exception)
			{
				return NotFound();
			}

			return Ok();
		}

		// PUT: update/5
		[Route("update")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, ListingDTO dto)
		{
			if (string.IsNullOrEmpty(id) || id != dto.Id)
			{
				return BadRequest();
			}

			try
			{
				// Ensures that all necessary entities exists in the database.
				await _companyService.AddAsync(new CompanyDTO { Name = dto.Employer });
				await _locationService.AddAsync(new LocationDTO { Name = dto.Location });
				await _experienceLevelService.AddAsync(new ExperienceLevelDTO { Name = dto.Location });

				await _listingService.UpdateAsync(dto, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// POST: /create
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
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		private string GetUserId()
		{
			string authHeaderValue = Request.Headers["Authorization"];

			var tokenClaims = GetClaims(authHeaderValue.Substring("Bearer ".Length).Trim());

			var userId = tokenClaims.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

			return userId;
		}

		public ClaimsPrincipal GetClaims(string token)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]));
			var handler = new JwtSecurityTokenHandler();
			var validations = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = key,
				ValidateIssuer = false,
				ValidateAudience = false
			};

			return handler.ValidateToken(token, validations, out _);
		}
	}
}
