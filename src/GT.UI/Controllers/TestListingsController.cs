using GT.Core.DTO;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Impl;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestListingsController : ControllerBase
	{
		private readonly IGTListingService _listingService;

		public TestListingsController(IGTListingService listingService)
		{
			_listingService = listingService;
		}

		// GET: api/<TestListingsController>
		[HttpGet]
		public async Task<IEnumerable<ListingPartialDTO>> Get()
		{
			var filter = new ListingFilterModel()
			{
				//	ExperienceLevel =
				//new List<string>
				//	{
				//		"Junior",
				//		"Banan",
				//		null
				//	},

				ExperienceLevels = new List<string> { null }
			};

			return await _listingService.GetAsync(filter);
		}

		// GET: api/<TestListingsController>
		[Route("tl/{id}")]
		[HttpGet]
		public async Task<ActionResult<ListingDTO>> Get(string id)
		{
			var listing = await _listingService.GetByIdAsync(id);

			if (listing is null)
			{
				return NotFound();
			}

			return listing;
		}

	}
}
