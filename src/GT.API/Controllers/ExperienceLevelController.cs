using GT.Core.DTO.ExperienceLevel;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GT.API.Controllers
{
	[Route("api/v1/experienceLevels")]
	[ApiController]
	public class ExperienceLevelController : Controller
	{
		private readonly IExperienceLevelService _experienceLevelService;

		public ExperienceLevelController(IExperienceLevelService experienceLevelService)
		{
			_experienceLevelService = experienceLevelService ?? throw new ArgumentNullException(nameof(experienceLevelService));
		}

		// GET:
		[HttpGet]
		public async Task<ActionResult<List<ExperienceLevelDTO>>> GetExperienceLevels()
		{
			try
			{
				var experienceLevels = await _experienceLevelService.GetAllAsync();

				return Ok(experienceLevels);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		//  GET: /5
		[HttpGet("{id}")]
		public async Task<ActionResult<ExperienceLevelDTO>> GetExperienceLevel(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				var experienceLevel = await _experienceLevelService.GetByIdAsync(id);

				if (experienceLevel is null)
				{
					return NotFound();
				}

				return Ok(experienceLevel);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// POST:
		[HttpPost]
		public async Task<ActionResult> PostExperienceLevel(PostExperienceLevelDTO experienceLevel)
		{
			if (experienceLevel is null)
			{
				return BadRequest();
			}

			try
			{
				if (!await _experienceLevelService.AddAsync(experienceLevel))
				{
					return StatusCode(500, "Failed to save ExperienceLevel to database.");
				}

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex);
			}
		}

		// PUT: /5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutExperienceLevel(string id, PostExperienceLevelDTO experienceLevel)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				if (!await _experienceLevelService.UpdateAsync(experienceLevel, id))
				{
					return StatusCode(500, "Failed to update ExperienceLevel to database.");
				}

				return Ok();
			}
			catch
			{
				return StatusCode(500);
			}
		}

		// DELETE: /5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteExperienceLevel(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				if (!await _experienceLevelService.DeleteAsync(id))
				{
					return StatusCode(500, "Failed to delete ExperienceLevel in database.");
				}

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}
