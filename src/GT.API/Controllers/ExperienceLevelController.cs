using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GT.API.Controllers
{
	[Route("api/v1/experienceLevel")]
	[ApiController]
	public class ExperienceLevelController : Controller
	{
		private readonly IGTExperienceLevelService _experienceLevelService;

		public ExperienceLevelController(IGTExperienceLevelService experienceLevelService)
		{
			_experienceLevelService = experienceLevelService ?? throw new ArgumentNullException(nameof(experienceLevelService));
		}

		// GET:/overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<string>> GetExperienceLevels()
		{
			var experienceLevel = await _experienceLevelService.GetAllAsync();

			if(experienceLevel == null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(experienceLevel);

			return result;
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
		[Route("update")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutExperienceLevel(string id, ExperienceLevelDTO experienceLevel)
		{
			if (string.IsNullOrEmpty(id) || id != experienceLevel.Id)
			{
				return BadRequest();
			}

			try
			{
				await _experienceLevelService.
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

	}
}
