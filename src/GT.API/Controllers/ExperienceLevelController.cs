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

		// GET: /overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<string>> GetExperienceLevels()
		{
			var experienceLevels = await _experienceLevelService.GetAllAsync();

			if (experienceLevels is null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(experienceLevels);

			return Ok(result);
		}

		//  GET: /5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetExperienceLevel(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var experienceLevel = await _experienceLevelService.GetByIdAsync(id);

			if (experienceLevel is null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(experienceLevel);

			return Ok(result);
		}

		// POST: /create
		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string?>> PostExperienceLevel(ExperienceLevelDTO experienceLevel)
		{
			if (experienceLevel is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _experienceLevelService.AddAsync(experienceLevel);
				var result = JsonConvert.SerializeObject(objToReturn);
				return StatusCode(201, result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		// PUT: update/5
		[Route("update/{id}")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutExperienceLevel(string id, ExperienceLevelDTO experienceLevel)
		{
			if (string.IsNullOrEmpty(id) || id != experienceLevel.Id)
			{
				return BadRequest();
			}

			try
			{
				await _experienceLevelService.UpdateAsync(experienceLevel, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// DELETE: delete/5
		[Route("delete/{id}")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteExperienceLevel(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				await _experienceLevelService.DeleteAsync(id);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}

			return Ok();
		}
	}
}
